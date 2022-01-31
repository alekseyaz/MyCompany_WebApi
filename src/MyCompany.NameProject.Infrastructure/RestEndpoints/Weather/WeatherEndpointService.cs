using Microsoft.Extensions.Options;
using MyCompany.NameProject.Application.Common.Interfaces;
using MyCompany.NameProject.Infrastructure.RestEndpoints.Weather.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using MyCompany.NameProject.Infrastructure.Cache;

namespace MyCompany.NameProject.Infrastructure.RestEndpoints.Weather
{
    public class WeatherEndpointService : IWeatherEndpointService
    {
        private static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        private readonly HttpClient _httpClient;
        private readonly WeatherEndpointSettings _settings;
        private readonly ILoggingService<WeatherEndpointService> _logger;
        private readonly ICacheService _cacheService;

        public WeatherEndpointService(
            HttpClient httpClient,
            IOptions<WeatherEndpointSettings> settings,
            ILoggingService<WeatherEndpointService> logger,
            ICacheService cacheService)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
            _logger = logger;
            _cacheService = cacheService;
        }

        public async Task<WeatherInformersResponse> InformersAsync(WeatherInformersRequest request, CancellationToken cancellationToken)
        {
            var parts = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("lat", request.Lat),
                new KeyValuePair<string, string>("lon", request.Lon),
                new KeyValuePair<string, string>("lang", request.Lang),
            };

            var url = $"{_settings.BaseApiUrl}";
            if (parts.Any())
            {
                var partQuery = parts
                    .Select(part => $"{part.Key}={HttpUtility.UrlEncode(part.Value)}")
                    .Aggregate((a, b) => a + "&" + b);

                url += "?" + partQuery;
            }

            var response = await GetAsync<WeatherInformersResponse>(
                url,
                _settings.AuthToken,
                _settings.CacheInterval,
                cancellationToken);

            return response;
        }

        private async Task<TResponse> GetAsync<TResponse>(
            string url, string token, TimeSpan? cacheInterval, CancellationToken cancellationToken)
        {
            try
            {
                var responseJson = await GetAsync(url, token, cacheInterval, cancellationToken);

                return !string.IsNullOrWhiteSpace(responseJson)
                    ? JsonSerializer.Deserialize<TResponse>(responseJson)
                    : default(TResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(url, ex, null);

                return default(TResponse);
            }
        }

        private async Task<TResponse> PostAsync<TRequest, TResponse>(
            string url, TRequest request, string token, TimeSpan? cacheInterval, CancellationToken cancellationToken)
        {
            try
            {
                var requestJson = JsonSerializer.Serialize(request, JsonSerializerOptions);
                var responseJson = await PostAsync(url, requestJson, token, cacheInterval, cancellationToken);

                return !string.IsNullOrWhiteSpace(responseJson)
                    ? JsonSerializer.Deserialize<TResponse>(responseJson)
                    : default(TResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(url, ex, null);

                return default(TResponse);
            }
        }
        private async Task<string> GetAsync(
            string url, string token, TimeSpan? cacheInterval, CancellationToken cancellationToken)
        {
            if (!cacheInterval.HasValue || cacheInterval.Value == TimeSpan.Zero)
                return await GetAsync(url, token, cancellationToken);

            var requestKeyParts = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("url", url),
                };

            var requestKey = EncodeBase64(requestKeyParts
                .Select(kvp => $"{kvp.Key}={kvp.Value}")
                .Aggregate((a, b) => a + "&" + b));

            return await _cacheService.GetCachedAsync(
                $"{GetType().Name}_Get_{requestKey}",
                async () => await GetAsync(url, token, cancellationToken),
                cacheInterval.Value);
        }

        private async Task<string> PostAsync(
            string url, string json, string token, TimeSpan? cacheInterval, CancellationToken cancellationToken)
        {
            if (!cacheInterval.HasValue || cacheInterval.Value == TimeSpan.Zero)
                return await PostAsync(url, json, token, cancellationToken);

            var requestKeyParts = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("url", url),
                    new KeyValuePair<string, string>("json", json)
                };

            var requestKey = EncodeBase64(requestKeyParts
                .Select(kvp => $"{kvp.Key}={kvp.Value}")
                .Aggregate((a, b) => a + "&" + b));

            return await _cacheService.GetCachedAsync(
                $"{GetType().Name}_Post_{requestKey}",
                async () => await PostAsync(url, json, token, cancellationToken),
                cacheInterval.Value);
        }

        private async Task<string> GetAsync(string url, string token, CancellationToken cancellationToken)
        {
            var successCodes = new[] { (int)HttpStatusCode.OK };

            var time = Stopwatch.StartNew();

            string responseString = null;
            try
            {
                var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
                if (!string.IsNullOrEmpty(token))
                    requestMessage.Headers.TryAddWithoutValidation("X-Yandex-API-Key", token);

                _logger.LogRequest(HttpMethod.Get, url, requestMessage);

                using var responseMessage = await _httpClient.SendAsync(requestMessage, cancellationToken);
                responseString = await GetResponseMessageContent(responseMessage);
                if (!successCodes.Contains((int)responseMessage.StatusCode))
                    responseMessage.EnsureSuccessStatusCode();
            }
            catch (TaskCanceledException ex)
            {
                time.Stop();
                _logger.LogError(url, ex, responseString, time.Elapsed,
                    $"exceeded timeout {_httpClient.Timeout.TotalMilliseconds} msec");

                return null;
            }
            catch (Exception ex)
            {
                time.Stop();
                _logger.LogError(url, ex, responseString, time.Elapsed);

                return null;
            }

            time.Stop();
            _logger.LogResponse(url, responseString, time.Elapsed);

            return responseString;
        }

        private async Task<string> PostAsync(string url, string json, string token, CancellationToken cancellationToken)
        {
            var successCodes = new[] { (int)HttpStatusCode.OK };

            var time = Stopwatch.StartNew();
            _logger.LogRequest(HttpMethod.Post, url, json);

            string responseString = null;
            try
            {
                var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
                if (!string.IsNullOrEmpty(token))
                    requestMessage.Headers.TryAddWithoutValidation("X-Yandex-API-Key", token);

                requestMessage.Content = new StringContent(json, Encoding.UTF8, "application/json");

                using var responseMessage = await _httpClient.SendAsync(requestMessage, cancellationToken);
                responseString = await GetResponseMessageContent(responseMessage);
                if (!successCodes.Contains((int)responseMessage.StatusCode))
                    responseMessage.EnsureSuccessStatusCode();
            }
            catch (TaskCanceledException ex)
            {
                time.Stop();
                _logger.LogError(url, ex, responseString, time.Elapsed,
                    $"exceeded timeout {_httpClient.Timeout.TotalMilliseconds} msec");

                return null;
            }
            catch (Exception ex)
            {
                time.Stop();
                _logger.LogError(url, ex, responseString, time.Elapsed);

                return null;
            }

            time.Stop();
            _logger.LogResponse(url, responseString, time.Elapsed);

            return responseString;
        }

        private async Task<string> GetResponseMessageContent(HttpResponseMessage response)
        {
            try
            {
                return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    response.RequestMessage.RequestUri.ToString(),
                    ex,
                    null,
                    null,
                    "can't read response content");

                return null;
            }
        }

        private static string EncodeBase64(string str)
        {
            return str != null ? Convert.ToBase64String(Encoding.UTF8.GetBytes(str)) : null;
        }
    }
}
