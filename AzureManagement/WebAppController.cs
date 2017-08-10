using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Azure.Management.AppService.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using AzureManagement.Logging.Interfaces;

namespace AzureManagement
{
    public class WebAppController
    {

        internal readonly ILogger Logger;
        public WebAppController(ILogger logger) { Logger = logger; }

        public IWebApp CreateWebAppProcess(ICloudAppConfig appconfig, IResourceGroup resgrp, IAppServicePlan plan)
        {
            Logger.Info("WebApp Creating");
            var webapp = Program.Cloud.WebApps.Define(appconfig.FullName)
                                .WithExistingWindowsPlan(plan)
                                .WithExistingResourceGroup(resgrp.Name.ToLower())
                                .WithClientAffinityEnabled(false)
                                .WithoutPhp()
                                .WithPhpVersion(PhpVersion.Off)
                                .WithPythonVersion(PythonVersion.Off)
                                .WithPlatformArchitecture(PlatformArchitecture.X64)
                                .WithWebSocketsEnabled(false)
                                .WithWebAppAlwaysOn(false)
                                .Create();
            Logger.Info("WebApp Created");

            CreateDeploymentSlot(webapp, appconfig.DeploymentSlotName);

            Logger.Info($"OutBound IP Range: {string.Join(",", webapp.OutboundIPAddresses)}");

            return webapp;
        }

        public void CreateDeploymentSlot(IWebApp webapp, string slotname)
        {
            if (webapp == null)
            {
                throw new ArgumentNullException(nameof(webapp));
            }
            if (slotname == null)
            {
                throw new ArgumentNullException(nameof(slotname));
            }

            Logger.Info("Deployment Slot - Creating");

            webapp.DeploymentSlots.Define(slotname)
                  .WithConfigurationFromWebApp(webapp)
                  .WithClientAffinityEnabled(false)
                  .Create();

            Logger.Info("Deployment Slot - Created");
        }

        public string GetAllWebAppsForAllResGrpsAsString(IResourceGroupController rgc)
        {
            var webapps = GetWebAppsFromAzure(rgc);
            var sb = new StringBuilder();
            foreach (var app in webapps)
            {
                sb.AppendLine(app.ToString());
            }

            return sb.ToString();

        }

        public IEnumerable<WebApp> GetWebAppsFromAzure(IResourceGroupController rgc)
        {
            var resourceGroups = rgc.ListAllResourGroups();
            var webapps = GetWebAppsForResourceGroupFromAzure(resourceGroups);
            return webapps;
        }

        public IEnumerable<WebApp> GetWebAppsForResourceGroupFromAzure(IEnumerable<IResourceGroup> resourceGrps)
        {
            Logger.Info("Get webapps for Resource Groups");
            var retval = new List<WebApp>();

            foreach (var resgrp in resourceGrps)
            {
                retval.AddRange(Program.Cloud.WebApps.ListByResourceGroup(resgrp.Name)
                                       .Select(app => new WebApp
                                       {
                                           Name = app.Name.ToUpper(),
                                           ResourceGroupName = resgrp.Name.ToUpper(),
                                           Urls = app.HostNames
                                       }));
            }

            return retval;
        }

        public void CreateAutoScalePolicyForApp(CloudAppConfig appconfig, IResourceGroup resgrp)
        {
            var asp = new AutoScalePolicy(Logger);
            var targetResUriServerFarm = $"/subscriptions/{appconfig.SubscriptionId}/resourceGroups/{appconfig.ResGrpName}/providers/Microsoft.Web/serverFarms/{appconfig.AppServicePlanName}";
            asp.CreateAutoscalePolicies(appconfig, targetResUriServerFarm, appconfig.FullName, resgrp.Name);
        }

        public void CreateAlertRulesForApp(CloudAppConfig appconfig, IResourceGroup resgrp)
        {
            IAlertRules alertRules = new AlertRules(Logger, appconfig);
            var targetResUriWebSite = $"/subscriptions/{appconfig.SubscriptionId}/resourceGroups/{appconfig.ResGrpName}/providers/Microsoft.Web/sites/{appconfig.FullName}/";
            alertRules.CreateAlertRule(resgrp, targetResUriWebSite);
        }
    }
}