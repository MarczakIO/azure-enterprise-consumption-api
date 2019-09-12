using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MarczakIO.EnterpriseAzure
{
    public static class RefreshBillingPeriod
    {
        private static TransientFaultClient Client { get; set; } = new TransientFaultClient();

        [FunctionName("RefreshBillingPeriod")]
        public static async Task Run(
            [QueueTrigger("billing-period-refreshes")] BillingPeriodRefresh refresh, 
            [Blob("billing/{EnterpriseAzureId}/{BillingPeriodId}/{Type}.json", FileAccess.Write)] Stream output,
            ILogger log)
        {
            log.LogInformation($"C# Processing period: {refresh.EnterpriseAzureId}");

            var kv = Environment.GetEnvironmentVariable("KeyVaultName");
            var secret = await KeyVaultManager.GetSecret(kv, refresh.EnterpriseAzureId);

            var headers = new Dictionary<string, string> { { "Authorization", $"Bearer {secret}" } };
            var url = $"https://consumption.azure.com/{refresh.URL}";
            var response = await Client.GetAsync(url, headers, log);
            var result = await response.Content.ReadAsStringAsync();

            using (var sr = new StreamWriter(output))
            {
                await sr.WriteAsync(result);
            }
            
            log.LogInformation($"Done.");
        }
    }
}
