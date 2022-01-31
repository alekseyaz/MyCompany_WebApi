using System;

namespace MyCompany.NameProject.Infrastructure.RestEndpoints.Common.Exceptions
{
    public class ApiException : Exception
    {
        public int? StatusCode { get; private set; }

        public string Response { get; private set; }

        public object Debug { get; set; }

        public ApiException(string message, int? statusCode, string response, Exception innerException, object debug = null)
            : base(message + "\n\nStatus: " + (statusCode ?? 0) + "\nResponse: \n" + response, innerException)
        {
            StatusCode = statusCode;
            Response = response;
            Debug = debug;
        }

        public override string ToString()
        {
            return $"HTTP Response: \n\n{Response}\n\n{base.ToString()}";
        }
    }
}
