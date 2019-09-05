using Microsoft.AspNetCore.Http;

namespace SampleDotNetCoreApplication.Models
{
    public class Error
    {
        public int StatusCode { get; set; }

        public string Message { get; set; }

        public Error(int statusCode, string message)
        {
            this.StatusCode = statusCode;
            this.Message = message;
        }

    }
}