using System;
using System.Collections.Generic;
using AzureManagement.Logging.Interfaces;
using Microsoft.Azure.Management.Insights;
using Microsoft.Azure.Management.Insights.Models;

namespace AzureManagement
{
    public class AutoScalePolicy
    {
        //TODO: move metric calcs to config ie memory and cpu thresholds, capactiy

        internal readonly ILogger Logger;
        public AutoScalePolicy(ILogger logger)
        {
            Logger = logger;
        }

        public void CreateAutoscalePolicies(ICloudAppConfig appConfig, string resourceUrl, string appName, string resGroupName)
        {
            Logger.Info("CreateAutoscalePolicies - Creating");
            var autoscaleconfig = (ICloudAppConfigAutoScaling)appConfig;
            var emailForScaling = autoscaleconfig.AutoScaleNotificationMailAddress;
            var capacity = CreateScaleCapacity();
            var rules = CreateRulesForAutoScaling(resourceUrl);
            var profiles = CreateAutoScaleProfile(capacity, rules);
            var notifications = CreateAutoscaleNotifications(emailForScaling);
            var autoscalesettingName = autoscaleconfig.AutoScaleSettingName;

            var asr = new AutoscaleSettingResource
            {
                Name = autoscalesettingName,
                AutoscaleSettingResourceName = autoscalesettingName,
                TargetResourceUri = resourceUrl,
                Enabled = true,
                Profiles = profiles,
                Notifications = notifications,
                Location = autoscaleconfig.AutoScaleResourceLocation

            };

            using (var imc = new InsightsManagementClient(appConfig.Creds) { SubscriptionId = appConfig.SubscriptionId })
            {
                imc.AutoscaleSettings.CreateOrUpdate(resGroupName, autoscalesettingName, asr);
            }

            Logger.Info("CreateAutoscalePolicies - Created");


        }


        private AutoscaleNotification[] CreateAutoscaleNotifications(IList<string> emailsList)
        {

            var notifications = new[]
            {
                new AutoscaleNotification(new EmailNotification(true, customEmails: emailsList))
            };
            return notifications;
        }

        private AutoscaleProfile[] CreateAutoScaleProfile(ScaleCapacity capacity, IList<ScaleRule> rules)
        {
            return new[]
            {
                new AutoscaleProfile()
                {
                    Name = "Auto created scale condition",
                    Capacity = capacity,
                    FixedDate = null,
                    Recurrence = null,
                    Rules = rules
                }
            };
        }

        private ScaleRule[] CreateRulesForAutoScaling(string resourceUrl)
        {
            var rules = new[]
            {
                CreateCpuIncreaseScaleRule(resourceUrl),
                CreateCpuDecreateScaleRule(resourceUrl),
                CreateMemoryIncreaseScaleRule(resourceUrl),
                CreateMemeoryDecreaseScaleRule(resourceUrl)

            };
            return rules;
        }

        private ScaleRule CreateMemeoryDecreaseScaleRule(string resourceUrl)
        {
            return new ScaleRule()
            {
                MetricTrigger = new MetricTrigger
                {
                    MetricName = "MemoryPercentage",
                    MetricResourceUri = resourceUrl,
                    Statistic = MetricStatisticType.Average,
                    Threshold = 40.0,
                    TimeAggregation = TimeAggregationType.Average,
                    TimeGrain = TimeSpan.FromMinutes(1),
                    TimeWindow = TimeSpan.FromMinutes(15),
                    OperatorProperty = ComparisonOperationType.LessThanOrEqual
                },
                ScaleAction = new ScaleAction
                {
                    Cooldown = TimeSpan.FromMinutes(10),
                    Direction = ScaleDirection.Decrease,
                    Value = "1",
                    Type = ScaleType.ChangeCount
                }
            };
        }

        private ScaleRule CreateMemoryIncreaseScaleRule(string resourceUrl)
        {
            return new ScaleRule()
            {
                MetricTrigger = new MetricTrigger
                {
                    MetricName = "MemoryPercentage",
                    MetricResourceUri = resourceUrl,
                    Statistic = MetricStatisticType.Average,
                    Threshold = 60.0,
                    TimeAggregation = TimeAggregationType.Average,
                    TimeGrain = TimeSpan.FromMinutes(1),
                    TimeWindow = TimeSpan.FromMinutes(15),
                    OperatorProperty = ComparisonOperationType.GreaterThanOrEqual
                },
                ScaleAction = new ScaleAction
                {
                    Cooldown = TimeSpan.FromMinutes(10),
                    Direction = ScaleDirection.Increase,
                    Value = "1",
                    Type = ScaleType.ChangeCount
                }
            };
        }

        private ScaleRule CreateCpuDecreateScaleRule(string resourceUrl)
        {
            return new ScaleRule()
            {
                MetricTrigger = new MetricTrigger
                {
                    MetricName = "CpuPercentage",
                    MetricResourceUri = resourceUrl,
                    Statistic = MetricStatisticType.Average,
                    Threshold = 40.0,
                    TimeAggregation = TimeAggregationType.Average,
                    TimeGrain = TimeSpan.FromMinutes(1),
                    TimeWindow = TimeSpan.FromMinutes(15),
                    OperatorProperty = ComparisonOperationType.LessThanOrEqual
                },
                ScaleAction = new ScaleAction
                {
                    Cooldown = TimeSpan.FromMinutes(10),
                    Direction = ScaleDirection.Decrease,
                    Value = "1",
                    Type = ScaleType.ChangeCount
                }
            };
        }

        private ScaleRule CreateCpuIncreaseScaleRule(string resourceUrl)
        {
            return new ScaleRule()
            {
                MetricTrigger = new MetricTrigger
                {
                    MetricName = "CpuPercentage",
                    MetricResourceUri = resourceUrl,
                    Statistic = MetricStatisticType.Average,
                    Threshold = 60.0,
                    TimeAggregation = TimeAggregationType.Average,
                    TimeGrain = TimeSpan.FromMinutes(1),
                    TimeWindow = TimeSpan.FromMinutes(15),
                    OperatorProperty = ComparisonOperationType.GreaterThanOrEqual
                },
                ScaleAction = new ScaleAction
                {
                    Cooldown = TimeSpan.FromMinutes(10),
                    Direction = ScaleDirection.Increase,
                    Value = "1",
                    Type = ScaleType.ChangeCount
                }
            };
        }

        private ScaleCapacity CreateScaleCapacity()
        {
            return new ScaleCapacity()
            {
                DefaultProperty = "1",
                Maximum = "10",
                Minimum = "1"
            };
        }

    }
}