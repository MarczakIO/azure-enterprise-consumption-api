using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MarczakIO.EnterpriseAzure
{
    public static class HttpTriggerCSharp
    {
        [FunctionName("HttpTriggerCSharp")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            [Queue("refreshes")] IAsyncCollector<Refresh> refreshes,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var eaIds = Environment.GetEnvironmentVariable("EnterpriseAzureIds");
            
            if (!string.IsNullOrEmpty(eaIds))
            foreach(var id in eaIds.Split(",")) 
                {
                    await refreshes.AddAsync(new Refresh()
                    {
                        EnterpriseAzureId = id,
                        RefreshType = RefreshType.Full
                    });
                }

            return new OkResult();
        }
    }
}
