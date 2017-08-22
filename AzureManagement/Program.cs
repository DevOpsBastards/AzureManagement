using AzureManagement.CommandLine;
using AzureManagement.Logging.Interfaces;
using AzureManagement.Logging.Services;
using Microsoft.Azure.Management.AppService.Fluent;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Azure.Management.ApiManagement;

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
        /* 
        SETX Azure-TenantID "<YOURTENANTID>"
        SETX Azure-SubscriptionID "<YOURSUBSCRIPTIONID>"
        SETX Azure-SubscriptionName "<YOURSUBSCRIPTIONNAME>"
        SETX Azure-SpnApplicationId "<YOURSPNID>"
        SETX Azure-SpnPassword "<YOURSPNPWD>"
        */
        /// </summary>
        /// <example>
        /// 
        /// </example>
        /// <param name="args"></param>
        /* 
        --Action "RSG" --apptype "Web" --envtype "Test" --appname "HUDSONWHEREISMYCARDUDE" --companyabbrv "DOB"
        --Action "APPPLAN" --apptype "Web" --envtype "Test" --appname "HUDSONWHEREISMYCARDUDE" --companyabbrv "DOB" --scaleemail "hudsonscale@test.com"
        --Action "WEBAPP" --apptype "Web" --envtype "Test" --appname "HUDSONWHEREISMYCARDUDE" --companyabbrv "DOB" --alertemail "hudsonAlert@test.com"
        */
        //            --Action "EVERYTHINGWEBAPP" --apptype "Web" --envtype "Test" --appname "HUDSONWHEREISMYCARDUDE" --companyabbrv "DOB" --scaleemail "hudsonscale@test.com" --alertemail "hudsonAlert@test.com"
        public static void Main(string[] args)
        {
            Logger.Info("Program startup");
            Logger.Info($"Program Name: {Assembly.GetExecutingAssembly().GetName(true).Name}");

            _clp = new CommandLineProcessor(args, Logger);

            //TODO: Add ability to override via command line args

            //Identify what we are doing today
            //Action: 
            //Create Resource Group

            //Create App Plan
            //autoscale policy

            //WebApp
            //alert

            //add Gateway
            //https://github.com/Azure/api-management-samples/tree/master/restApiDemo
            //https://docs.microsoft.com/en-us/azure/templates/microsoft.apimanagement/service/apis


            //Inputs needed: 

            var environmentTypeFromInput = Enumeration.FromDisplayName<EnvironmentType>(_clp.Options.EnvironmentName);
            var applicationTypeFromInput = Enumeration.FromDisplayName<ApplicationType>(_clp.Options.ApplicationTypeName);
            var actionTypeFromInput = Enumeration.FromDisplayName<ActionTypes>(_clp.Options.Action);

            IAzureConfig config = new CloudConfig(Logger);
            Cloud = Azure.Authenticate(config.GetCredentials()).WithDefaultSubscription();

            if ((actionTypeFromInput == null) || (actionTypeFromInput.Value == 99))
                throw new ArgumentNullException("Action cannot be null", _clp.Options.Action);

            var companyAbbrevPrefix = !string.IsNullOrEmpty(_clp.Options.CompanyAbbrevPrefix) ? _clp.Options.CompanyAbbrevPrefix : System.Configuration.ConfigurationManager.AppSettings["companyabbrvPrefix"];
            var resgrpNameSuffix = System.Configuration.ConfigurationManager.AppSettings["resgrpNameSuffix"];
            var appServicePlanNameSuffix = System.Configuration.ConfigurationManager.AppSettings["appServicePlanNameSuffix"];
            var autoScaleResourceLocation = System.Configuration.ConfigurationManager.AppSettings["autoScaleResourceLocation"];
            var deploymentSlotName = System.Configuration.ConfigurationManager.AppSettings["deploymentSlotName"];
            var appInsightsLocation = System.Configuration.ConfigurationManager.AppSettings["appInsightsLocation"];
            var ruleNameSuffix = System.Configuration.ConfigurationManager.AppSettings["ruleNameSuffix"];
            var defaultRegion = System.Configuration.ConfigurationManager.AppSettings["defaultRegion"];
            var defaultPricingTier = System.Configuration.ConfigurationManager.AppSettings["defaultPricingTier"];
            var appInsightsNameSuffix = System.Configuration.ConfigurationManager.AppSettings["AppInsightsNameSuffix"];
            var regFromString = (Region)typeof(Region).GetField(defaultRegion).GetValue(typeof(Region));
            var pricingTierFromString = (PricingTier)typeof(PricingTier).GetField(defaultPricingTier).GetValue(typeof(PricingTier));

            var autoscaleEmail = !string.IsNullOrEmpty(_clp.Options.AutoScaleNotificationEmails) ? _clp.Options.AutoScaleNotificationEmails : System.Configuration.ConfigurationManager.AppSettings["scaleemail"];
            var errorEmail = !string.IsNullOrEmpty(_clp.Options.RuleAlertEmails) ? _clp.Options.RuleAlertEmails : System.Configuration.ConfigurationManager.AppSettings["alertemail"]; ;


            var appconfig = new CloudAppConfig
            {
                CompanyAbbrevPrefix = companyAbbrevPrefix,
                Environment = environmentTypeFromInput,
                ApplicationType = applicationTypeFromInput,
                BaseAppName = _clp.Options.ApplicationBaseName,
                DeploymentSlotName = deploymentSlotName,
                DefaultRegion = regFromString,
                ResgrpNameSuffix = resgrpNameSuffix,
                AppServicePlanNameSuffix = appServicePlanNameSuffix,
                AutoScaleResourceLocation = autoScaleResourceLocation,
                AuotScaleNotificationMailAddress = new List<string>() { autoscaleEmail },
                DefaultPricingTier = pricingTierFromString,
                AppInsightsLocation = appInsightsLocation,
                RuleNameSuffix = ruleNameSuffix,
                RuleAlertMailAddress = new List<string>() { errorEmail },
                AppInsightsNameSuffix = appInsightsNameSuffix,
                Creds = config.GetCredentials(),
                SubscriptionId = Cloud.SubscriptionId
            };

            var resGrpController = new ResourceGroupController(Logger);

            if (actionTypeFromInput.Equals(ActionTypes.CreateResGroup))
            {
                var resgrp = resGrpController.CreateResourceGroup(appconfig.ResGrpName, appconfig.DefaultRegion);

            }

            if (actionTypeFromInput.Equals(ActionTypes.CreateAppPlan))
            {
                var resgrp = resGrpController.GetResourceGroupByName(appconfig.ResGrpName);
                var plan1 = resGrpController.CreateAppServicePlan(appconfig, resgrp);
                resGrpController.CreateApplicationInsights(appconfig);
            }

            if (actionTypeFromInput.Equals(ActionTypes.CreateWebApp))
            {
                var resgrp = resGrpController.GetResourceGroupByName(appconfig.ResGrpName);
                var plan1 = resGrpController.GetAppPlanByName(appconfig.ResGrpName, appconfig.AppServicePlanName);
                var webappController = new WebAppController(Logger);
                webappController.CreateWebAppProcess(appconfig, resgrp, plan1);
                webappController.CreateAlertRulesForApp(appconfig, resgrp);
                webappController.CreateAutoScalePolicyForApp(appconfig, resgrp);
            }

            //TODO: implement apigateway 
            ////http://zzz-test-svc-AppNAME.azurewebsites.net/swagger/v1/swagger.json
            //var apiGatewayName = "testgateway";
            //var apiGatewayResGrpName = "Api-Default-East-US-2";
            //var apiName = "Api.HudsonTest";
            //var routeName = "HudsonTest";
            //var serviceUrl = "http://test-web-hudsonwhereismycardude.azurewebsites.net";
            //var productToAdd = "Microservice";
            //var swaggerUrl = $"{serviceUrl}/swagger/v1/swagger.json";
            //var amc = new ApiManagementController(config, Logger, apiGatewayName, apiGatewayResGrpName);
            //amc.OrchestrateApiAddedToApiManagementService(apiName, routeName, serviceUrl, swaggerUrl, productToAdd);


            //if (!string.IsNullOrEmpty(_clp.Options.OutFilePath))
            //{
            //    var webapps = webappController.GetAllWebAppsForAllResGrpsAsString(resGrpController);
            //    UtilityFile.WriteOutputFile(webapps, _clp.Options.OutFilePath);
            //}

            Logger.Info("Program End");

            if (_clp.Options.IsDebug)
                Console.ReadLine();

        }




    }
}
