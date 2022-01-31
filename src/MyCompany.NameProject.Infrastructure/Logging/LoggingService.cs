using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using MyCompany.NameProject.Application.Common.Interfaces;

namespace MyCompany.NameProject.Infrastructure.Logging
{
    public class LoggingService<T> : ILoggingService<T>
    {
        private readonly string _clientName;
        private readonly ILogger<T> _logger;

        public LoggingService(ILogger<T> logger)
        {
            _logger = logger;
            _clientName = SetName();
        }

        private static string SetName()
        {
            var name = typeof(T).Name;

            return new[] {"service", "client", "repository"}.Aggregate(name, (current, cutString) => current.Replace(cutString, "", StringComparison.InvariantCultureIgnoreCase));
        }

        public void LogRequest(HttpMethod method, string path, object requestParams)
        {
            var requestParamsString = requestParams != null
                ? JsonSerializer.Serialize(requestParams)
                : null;

            LogRequest(method, path, requestParamsString);
        }

        public void LogRequest(HttpMethod method, string path, string request)
        {
            var requestLog = new List<string>
            {
                $"{_clientName} request",
                $"method: {method.Method.ToUpper()}",
                $"path: '{path}'",
                $"data: '{request}'"
            };

            _logger?.LogInformation(string.Join(", ", requestLog));
        }

        public void LogResponse(string path, object responseData, TimeSpan processingTimespan)
        {
            LogResponse(
                path,
                JsonSerializer.Serialize(new
                {
                    responseData,
                    processingTimespan
                }),
                processingTimespan
            );
        }

        public void LogResponse(string path, string response, TimeSpan processingTimespan)
        {
            var responseLog = new List<string>
            {
                $"{_clientName} response",
                $"path: '{path}'",
                $"data: '{response}'",
                $"time: '{processingTimespan}'"
            };
            _logger?.LogInformation(string.Join(", ", responseLog));
        }

        public void LogError(string path, Exception ex, object response, TimeSpan? processingTimespan = null, string message = null)
        {
            LogError(path, ex, JsonSerializer.Serialize(response), processingTimespan, message);
        }

        public void LogError(string path, Exception ex, string response, TimeSpan? processingTimespan = null, string message = null)
        {
            var errorLog = new List<string>();

            var additionalMessage = "";
            if (!string.IsNullOrEmpty(message))
                additionalMessage = $" {message}";

            errorLog.Add($"{_clientName} error{additionalMessage}");
            errorLog.Add($"path: '{path}'");
            errorLog.Add($"data: '{response}'");

            if (processingTimespan != null)
                errorLog.Add($", time: '{processingTimespan}'");

            _logger?.LogError(ex, string.Join(", ", errorLog));
        }
    }
}
