using System;
using System.Threading.Tasks;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.WindowsAzure.Storage.Table;

namespace MarczakIO.EnterpriseAzure
{
    public class BillingPeriodRefresh
    {
        public string EnterpriseAzureId { get; set; }
        public string BillingPeriodId { get; set; }
        public string URL { get; set; }
        public string Type { get; set; }
    }
}