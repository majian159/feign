using System;
using System.IO;
using System.Net.Http;

namespace Rabbit.Feign
{
    public class FeignException : Exception
    {
        private int _status;

        protected FeignException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected FeignException(string message) : base(message)
        {
        }

        protected FeignException(int status, string message) : base(message)
        {
            _status = status;
        }

        public static FeignException ErrorStatus(string methodKey, HttpResponseMessage response)
        {
            var status = (int)response.StatusCode;
            var message = $"status {status}({response.StatusCode}) reading {methodKey}";

            try
            {
                if (response.Content != null)
                {
                    var body = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    message += "; content:\n" + body;
                }
            }
            catch (IOException)
            {
            }
            return new FeignException(status, message);
        }
    }
}