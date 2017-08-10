using AzureManagement.CommandLine;
using AzureManagement.Logging.Interfaces;
using AzureManagement.Logging.Services;
using Microsoft.Azure.Management.AppService.Fluent;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace AzureManagement
{
    //https://cmatskas.com/the-new-azure-management-fluent-api-is-has-landed/
    public static class Program
    {
        private static readonly ILogger Logger = new LoggingService();
        private static CommandLineProcessor _clp;
        public static IAzure Cloud;

        //TODO: 
        //generate secrets file
        //Subscription topics
        //Docdbs
        //BlobStorage
        //Sqlserver dbs
        //push to config service

        /// <summary>
        /// 
        /// </summary>
        /// <example>
        /// --apptype = "Web" --envtype = "Test" --companyabbrv "HUDSONWHEREISMYCARDUDE" --companyabbrv "IWG" --scaleemail  "hudsonscale@test.com"; --alertemail "hudsonAlert@test.com";
        /// </example>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            Logger.Info("Program startup");
            Logger.Info($"Program Name: {Assembly.GetExecutingAssembly().GetName(true).Name}");

            _clp = new CommandLineProcessor(args, Logger);
            

            //Inputs needed: 
            
            var environmentTypeFromInput = Enumeration.FromDisplayName<EnvironmentType>(_clp.Options.EnvironmentName);
            var applicationTypeFromInput = Enumeration.FromDisplayName<ApplicationType>(_clp.Options.ApplicationTypeName);

            ICloudConfig config = new CloudConfig(Logger);
            Cloud = Azure.Authenticate(config.GetCredentials()).WithDefaultSubscription();



            // var configController = new ConfigController(Logger);
            //var azureManagementClientConfig = configController.CreateAzureManagementClientConfig(config);


            //standards from  appconfig
            var resgrpNameSuffix = System.Configuration.ConfigurationManager.AppSettings["resgrpNameSuffix"];
            var appServicePlanNameSuffix = System.Configuration.ConfigurationManager.AppSettings["appServicePlanNameSuffix"];
            var autoScaleResourceLocation = System.Configuration.ConfigurationManager.AppSettings["autoScaleResourceLocation"];
            var deploymentSlotName = System.Configuration.ConfigurationManager.AppSettings["deploymentSlotName"];
            var appInsightsLocation = System.Configuration.ConfigurationManager.AppSettings["appInsightsLocation"];
            var ruleNameSuffix = System.Configuration.ConfigurationManager.AppSettings["ruleNameSuffix"];
            var defaultRegion = System.Configuration.ConfigurationManager.AppSettings["defaultRegion"];
            var defaultPricingTier = System.Configuration.ConfigurationManager.AppSettings["defaultPricingTier"];
            var appInsightsNameSuffix = System.Configuration.ConfigurationManager.AppSettings["AppInsightsNameSuffix"];

            //TODO: Add ability to override via command line args


            var regFromString = (Region)typeof(Region).GetField(defaultRegion).GetValue(typeof(Region));
            var pricingTierFromString = (PricingTier)typeof(PricingTier).GetField(defaultPricingTier).GetValue(typeof(PricingTier));

            var appconfig = new CloudAppConfig
            {
                CompanyAbbrevPrefix = _clp.Options.CompanyAbbrevPrefix,
                Environment = environmentTypeFromInput,
                ApplicationType = applicationTypeFromInput,
                BaseAppName = _clp.Options.ApplicationBaseName,
                DeploymentSlotName = deploymentSlotName,
                DefaultRegion = regFromString,
                ResgrpNameSuffix = resgrpNameSuffix,
                AppServicePlanNameSuffix = appServicePlanNameSuffix,
                AutoScaleResourceLocation = autoScaleResourceLocation,
                AuotScaleNotificationMailAddress = new List<string>() { _clp.Options.AutoScaleNotificationEmails },
                DefaultPricingTier = pricingTierFromString,
                AppInsightsLocation = appInsightsLocation,
                RuleNameSuffix = ruleNameSuffix,
                RuleAlertMailAddress = new List<string>() { _clp.Options.RuleAlertEmails },
                AppInsightsNameSuffix = appInsightsNameSuffix,
                Creds = config.GetCredentials(),
                SubscriptionId = Cloud.SubscriptionId
            };

            var resGrpController = new ResourceGroupController(Logger);
            var resgrp = resGrpController.CreateResourceGroup(appconfig.ResGrpName, appconfig.DefaultRegion);
            var plan1 = resGrpController.CreateAppServicePlan(appconfig, resgrp);
            resGrpController.CreateApplicationInsights(appconfig);


            var webappController = new WebAppController(Logger);
            webappController.CreateWebAppProcess(appconfig, resgrp, plan1);
            webappController.CreateAlertRulesForApp(appconfig, resgrp);
            webappController.CreateAutoScalePolicyForApp(appconfig, resgrp);


            if (!string.IsNullOrEmpty(_clp.Options.OutFilePath))
            {
                var webapps = webappController.GetAllWebAppsForAllResGrpsAsString(resGrpController);
                UtilityFile.WriteOutputFile(webapps, _clp.Options.OutFilePath);
            }

            Logger.Info("Program End");

            if (_clp.Options.IsDebug)
                Console.ReadLine();

        }




    }
}
