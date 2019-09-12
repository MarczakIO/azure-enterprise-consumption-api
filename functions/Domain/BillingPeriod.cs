using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.WindowsAzure.Storage.Table;

namespace MarczakIO.EnterpriseAzure
{
    public class BillingPeriod
    {
        public string EnterpriseAzureId { get; set; }
        public string BillingPeriodId { get; set; }
        public DateTime BillingStart { get; set; }
        public DateTime billingEnd { get; set; }
        public string BalanceSummary { get; set; }
        public string UsageDetails { get; set; }
        public string MarketplaceCharges { get; set; }
        public string PriceSheet { get; set; }

        public List<BillingPeriodRefresh> GetRefreshes()
        {
            var refreshes = new List<BillingPeriodRefresh>();

            if (!string.IsNullOrEmpty(BalanceSummary))
                refreshes.Add(new BillingPeriodRefresh(){
                    BillingPeriodId = BillingPeriodId,
                    URL = BalanceSummary,
                    EnterpriseAzureId = EnterpriseAzureId,
                    Type = nameof(BalanceSummary)
                });
            if (!string.IsNullOrEmpty(UsageDetails))
                refreshes.Add(new BillingPeriodRefresh(){
                    BillingPeriodId = BillingPeriodId,
                    URL = UsageDetails,
                    EnterpriseAzureId = EnterpriseAzureId,
                    Type = nameof(UsageDetails)
                });
            if (!string.IsNullOrEmpty(MarketplaceCharges))
                refreshes.Add(new BillingPeriodRefresh(){
                    BillingPeriodId = BillingPeriodId,
                    URL = MarketplaceCharges,
                    EnterpriseAzureId = EnterpriseAzureId,
                    Type = nameof(MarketplaceCharges)
                });
            if (!string.IsNullOrEmpty(PriceSheet))
                refreshes.Add(new BillingPeriodRefresh(){
                    BillingPeriodId = BillingPeriodId,
                    URL = PriceSheet,
                    EnterpriseAzureId = EnterpriseAzureId,
                    Type = nameof(PriceSheet)
                });

            return refreshes;
        }
    }
}