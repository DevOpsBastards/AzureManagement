using System.Collections.Generic;
using AzureManagement.Logging.Interfaces;
using Microsoft.Azure.Management.AppService.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
using Microsoft.Azure.Management.ResourceManager.Fluent.Models;
using Newtonsoft.Json.Linq;

namespace AzureManagement
{
    public interface IResourceGroupController
    {
        IResourceGroup CreateResourceGroup(string resName, Region region);

        IResourceGroup GetResourceGroupByName(string resName);

        IEnumerable<IResourceGroup> ListAllResourGroups();
        string CreateApplicationInsights(ICloudAppConfig appConfig);
        IAppServicePlan CreateAppServicePlan(ICloudAppConfig appconfig, IResourceGroup resgrp);
        IAppServicePlan GetAppPlanByName(string resourceGroupName, string appServicePlanName);
    }

    public class ResourceGroupController : IResourceGroupController
    {
        internal readonly ILogger Logger;
        public ResourceGroupController(ILogger logger)
        {
            Logger = logger;

        }

        public IResourceGroup CreateResourceGroup(string resName, Region region)
        {
            Logger.Info("Resource Group - Creating");
            var resgrp = Program.Cloud.ResourceGroups.Define(resName)
                                .WithRegion(region)
                                //.WithTag("FrontEnd", "Website")
                                .Create();

            Logger.Info("Resource Group - Created");
            return resgrp;

        }

        public IResourceGroup GetResourceGroupByName(string resName)
        {
            return Program.Cloud.ResourceGroups.GetByName(resName);
        }

        public IEnumerable<IResourceGroup> ListAllResourGroups()
        {
            Logger.Info("Get Resource Groups");
            return Program.Cloud.ResourceGroups.List();
        }

        public string CreateApplicationInsights(ICloudAppConfig appConfig)
        {
            using (var rmc = new ResourceManagementClient(appConfig.Creds))
            {
                rmc.SubscriptionId = appConfig.SubscriptionId;
                var myparams = new GenericResourceInner(appConfig.AppInsightsLocation,
                                                        name: appConfig.AppInsightsName,
                                                        properties: new
                                                        {
                                                            ApplicationId = appConfig.AppInsightsName
                                                        });

                var appinsights = rmc.Resources.CreateOrUpdate(appConfig.ResGrpName,
                                                               resourceProviderNamespace: "microsoft.insights",
                                                               resourceType: "components",
                                                               resourceName: appConfig.AppInsightsName,
                                                               parentResourcePath: "",
                                                               apiVersion: "2015-05-01",
                                                               parameters: myparams);

                var key = JObject.Parse(appinsights.Properties.ToString())["InstrumentationKey"];

                Logger.Info($"AppInsightKey={key}");

                return key.ToString();
            }

        }

        public IAppServicePlan CreateAppServicePlan(ICloudAppConfig appconfig, IResourceGroup resgrp)
        {
            Logger.Info("App Plan - Creating");

            var appPlan = Program.Cloud.AppServices.AppServicePlans.Define(appconfig.AppServicePlanName)
                                 .WithRegion(resgrp.Region)
                                 .WithExistingResourceGroup(resgrp)
                                 .WithPricingTier(appconfig.DefaultPricingTier)
                                 .WithOperatingSystem(appconfig.DefaultOperatingSystem)
                                 .WithCapacity(1)
                                 .Create();

            Logger.Info("App Plan - Created");
            return appPlan;
        }

        public IAppServicePlan GetAppPlanByName(string resourceGroupName, string appServicePlanName)
        {
            var appPlan = Program.Cloud.AppServices.AppServicePlans.GetByResourceGroup(resourceGroupName, appServicePlanName);
            return appPlan;

        }
    }
}