using System;
using System.Collections.Generic;
using System.Net.Mail;
using Microsoft.Azure.Management.AppService.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
using Microsoft.Rest;
using OperatingSystem = Microsoft.Azure.Management.AppService.Fluent.OperatingSystem;

namespace AzureManagement
{
    public interface ICloudAppConfig
    {
        string CompanyAbbrevPrefix { get; set; }
        string ResgrpNameSuffix { get; set; }
        string AppServicePlanNameSuffix { get; set; }
        EnvironmentType Environment { get; set; }
        ApplicationType ApplicationType { get; set; }
        string BaseAppName { get; set; }
        string FullName { get; }
        string ResGrpName { get; }
        string AppServicePlanName { get; }
        string DeploymentSlotName { get; set; }

        Region DefaultRegion { get; set; }

        PricingTier DefaultPricingTier { get; set; }

        OperatingSystem DefaultOperatingSystem { get; set; }

        string AppInsightsLocation { get; set; }

        string AppInsightsNameSuffix { get; set; }
        string AppInsightsName { get; }
        // ICloudAppConfigAutoScaling AutoScalingConfig { get; set; }
        // IAlertRuleConfiguration AlertConfig { get; set; }
        ServiceClientCredentials Creds { get; set; }
        string SubscriptionId { get; set; }

    }

    public interface IAzureManagementClientConfiguration
    {
        ServiceClientCredentials Creds { get; set; }
        string SubscriptionId { get; set; }
    }
    public interface ICloudAppConfigAutoScaling
    {
        List<string> AutoScaleNotificationMailAddress { get; set; }
        string AutoScaleSettingNameSuffix { get; set; }
        string AutoScaleSettingName { get; }
        string AutoScaleResourceLocation { get; set; }
    }

    public interface IAlertRuleConfiguration
    {
        List<string> RuleAlertMailAddress { get; set; }
        string RuleNameSuffix { get; set; }
        string RuleName { get; }

    }


    public class CloudAppConfig : ICloudAppConfig, IAlertRuleConfiguration, ICloudAppConfigAutoScaling, IAzureManagementClientConfiguration
    {
        public List<string> AuotScaleNotificationMailAddress { get; set; }
        public string CompanyAbbrevPrefix { get; set; }
        public string ResgrpNameSuffix { get; set; }
        public string AppServicePlanNameSuffix { get; set; }
        public List<string> AutoScaleNotificationMailAddress { get; set; }
        public string AutoScaleSettingNameSuffix { get; set; }

        public EnvironmentType Environment { get; set; }
        public ApplicationType ApplicationType { get; set; }
        public string BaseAppName { get; set; }
        public Region DefaultRegion { get; set; }
        public PricingTier DefaultPricingTier { get; set; }
        public OperatingSystem DefaultOperatingSystem { get; set; }
        public string AppInsightsLocation { get; set; }
        public string AppInsightsNameSuffix { get; set; }
        public string AppInsightsName => $"{FullName}-{AppInsightsNameSuffix}";

        public string DeploymentSlotName { get; set; }

        public string FullName => $"{CompanyAbbrevPrefix}-{Environment.DisplayName}-{ApplicationType.DisplayName}-{BaseAppName}".ToUpper();
        public string ResGrpName => $"{FullName}-{ResgrpNameSuffix}";
        public string AppServicePlanName => $"{FullName}-{AppServicePlanNameSuffix}";

        public string AutoScaleSettingName => $"{FullName}-{AutoScaleSettingNameSuffix}";
        public string AutoScaleResourceLocation { get; set; }

        public List<string> RuleAlertMailAddress { get; set; }
        public string RuleNameSuffix { get; set; }
        public string RuleName => $"{FullName}-{RuleNameSuffix}";

        public ServiceClientCredentials Creds { get; set; }
        public string SubscriptionId { get; set; }




    }
}
