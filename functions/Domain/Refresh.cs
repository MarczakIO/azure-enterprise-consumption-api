using System;
using System.Threading.Tasks;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.WindowsAzure.Storage.Table;

namespace MarczakIO.EnterpriseAzure
{
    public class Refresh
    {
        public string EnterpriseAzureId { get; set; }
        public string RefreshType { get; set; }
    }
}