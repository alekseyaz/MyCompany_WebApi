using System;
using System.Net.Http;

namespace MyCompany.NameProject.Application.Common.Interfaces
{
    public interface ILoggingService<T>
    {
        void LogRequest(HttpMethod method, string path, object requestParams);

        void LogRequest(HttpMethod method, string path, string request);

        void LogResponse(string path, object responseData, TimeSpan processingTimespan);

        void LogResponse(string path, string response, TimeSpan processingTimespan);

        void LogError(string path, Exception ex, object response, TimeSpan? processingTimespan = null, string message = null);

        void LogError(string path, Exception ex, string response, TimeSpan? processingTimespan = null, string message = null);
    }
}
