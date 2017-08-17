using AzureManagement.Logging.Interfaces;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
using System;
using System.Collections.Generic;

namespace AzureManagement
{
    //TODO: way to big break it down!!!!
    public interface IAzureConfig
    {
        string TenantId { get; set; }
        string SubscriptionId { get; set; }
        string SubscriptionName { get; set; }
        string SpnApplicationId { get; set; } //automated account like jenkins
        string SpnPassword { get; set; } //App Registration //app/settings//keys//authkey
        AzureCredentials GetCredentials();
        void LoadConfigFromEnvironmentVariables();
    }

    public class CloudConfig : IAzureConfig
    {
        internal readonly ILogger Logger;
        public string TenantId { get; set; }
        public string SubscriptionId { get; set; }
        public string SubscriptionName { get; set; }
        public string SpnApplicationId { get; set; } //automated account like jenkins
        public string SpnPassword { get; set; } //App Registration //app/settings//keys//authkey

        public CloudConfig(ILogger logger)
        {
            Logger = logger;
            LoadConfigFromEnvironmentVariables();
        }

        public AzureCredentials GetCredentials()
        {
            Logger.Info("Azure Credentials - Start");
            var credentials = new AzureCredentials(new ServicePrincipalLoginInformation()
            {
                ClientId = SpnApplicationId,
                ClientSecret = SpnPassword
            }, TenantId, AzureEnvironment.AzureGlobalCloud);
            Logger.Info("Azure Credentials - End");
            return credentials;
        }

        public void LoadConfigFromEnvironmentVariables()
        {
            Logger.Info("Environment Variables - loading");
            //SETX TenantID "YourTenID"
            var envVarList = new List<string>
            {
                "Azure-TenantID",
                "Azure-SubscriptionID",
                "Azure-SubscriptionName",
                "Azure-SpnApplicationId",
                "Azure-SpnPassword"
            };

            foreach (var envvar in envVarList)
            {
                if (Environment.GetEnvironmentVariable(envvar) == null)
                {
                    throw new ArgumentNullException(envvar, $"{envvar} is not set as an environment variable");
                }
            }

            TenantId = Environment.GetEnvironmentVariable("Azure-TenantID");
            SubscriptionId = Environment.GetEnvironmentVariable("Azure-SubscriptionID");
            SubscriptionName = Environment.GetEnvironmentVariable("Azure-SubscriptionName");
            SpnApplicationId = Environment.GetEnvironmentVariable("Azure-SpnApplicationId");
            SpnPassword = Environment.GetEnvironmentVariable("Azure-SpnPassword");
            Logger.Info("Environment Variables - Done");
        }



    }
}
