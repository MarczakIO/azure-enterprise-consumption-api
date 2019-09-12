using Microsoft.Extensions.Logging;
using Polly;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MarczakIO.EnterpriseAzure
{
    public class TransientFaultClient : IDisposable
    {
        private HttpClient Client { get; set; }
        public int MyProperty { get; set; }

        public TransientFaultClient()
        {
            Client = BuildClient();
        }

        public async Task<HttpResponseMessage> GetAsync(string url, IDictionary<string, string> headers, ILogger log)
        {
            var policy = Policy
                .Handle<HttpRequestException>()
                .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                .WaitAndRetryAsync(
                5,
                attempt => TimeSpan.FromSeconds(0.5 * Math.Pow(2, attempt)),
                (exception, calculatedWaitDuration) =>
                {
                    if (exception.Exception != null)
                        log.LogInformation($"Failed request. Exception {exception.Exception.Message}");
                    if (exception.Result != null)
                        log.LogInformation($"Failed request. Code {exception.Result.StatusCode}");
                });

            HttpResponseMessage response = await policy.ExecuteAsync(async () =>
            {
                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri(url),
                    Method = HttpMethod.Get
                };
                foreach(var header in headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
                
                response = await Client.SendAsync(request);
                return response;
            });

            return response;
        }

        private HttpClient BuildClient()
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }

        public void Dispose()
        {
            if (Client != null)
            {
                Client.Dispose();
            }
        }
    }
}