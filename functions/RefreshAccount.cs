using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MarczakIO.EnterpriseAzure
{
    public static class RefreshAccount
    {
        private static TransientFaultClient Client { get; set; } = new TransientFaultClient();

        [FunctionName("RefreshAccount")]
        public static async Task Run(
            [QueueTrigger("refreshes")] Refresh refresh, 
            [Queue("billing-period-refreshes")] IAsyncCollector<BillingPeriodRefresh> billingPeriodRefreshes,
            ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {refresh.EnterpriseAzureId}");

            var kv = Environment.GetEnvironmentVariable("KeyVaultName");
            var secret = await KeyVaultManager.GetSecret(kv, refresh.EnterpriseAzureId);

            var headers = new Dictionary<string, string> { { "Authorization", $"Bearer {secret}" } };
            var url = $"https://consumption.azure.com/v2/enrollments/{refresh.EnterpriseAzureId}/billingperiods";
            var response = await Client.GetAsync(url, headers, log);
            var resultContent = await response.Content.ReadAsStringAsync();

            var billingPeriods = JsonConvert.DeserializeObject<List<BillingPeriod>>(resultContent);

            var i = 0;
            foreach(var billingPeriod in billingPeriods) {
                billingPeriod.EnterpriseAzureId = refresh.EnterpriseAzureId;
                
                foreach(var billingPeriodRefresh in billingPeriod.GetRefreshes()) {
                    await billingPeriodRefreshes.AddAsync(billingPeriodRefresh);
                }

                i++;
            }

            log.LogInformation($"response: {resultContent}");
        }
    }
}
