using System;
using System.Threading.Tasks;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.WindowsAzure.Storage.Table;

namespace MarczakIO.EnterpriseAzure
{
    public class KeyVaultManager
    {
        public static async Task<string> GetSecret(string keyVaultName, string secretName) 
        {
            string secretUrl = $"https://{keyVaultName}.vault.azure.net/secrets/{secretName}";
            AzureServiceTokenProvider azureServiceTokenProvider = new AzureServiceTokenProvider();
            var keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));
            var secretObject = await keyVaultClient.GetSecretAsync(secretUrl).ConfigureAwait(false);
            string secret = secretObject.Value;
            return secret;
        }
    }
}