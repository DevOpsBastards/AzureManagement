using System;
using System.Collections.Generic;
using AzureManagement.Logging.Interfaces;
using Microsoft.Azure.Management.Insights;
using Microsoft.Azure.Management.Insights.Models;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Rest;

namespace AzureManagement
{
    public interface IAlertRules
    {
        AlertRuleResource CreateAlertRule(IResourceGroup resgrp, string targetResUriWebSite);
    }

    public class AlertRules : IAlertRules
    {
        private readonly ILogger _logger;
        internal readonly IAzureManagementClientConfiguration AzureManagementClientConfiguration;
        //internal readonly IAlertRuleConfiguration AlertRuleConfig;
        private readonly ICloudAppConfig _appconfig;

        //public AlertRules(ILogger logger, ICloudAppConfig appconfig, IAlertRuleConfiguration alertRuleConfig)
        //{
        //    Logger = logger;
        //    _appconfig = appconfig;
        //    AzureManagementClientConfiguration = (IAzureManagementClientConfiguration)appconfig;
        //    AlertRuleConfig = alertRuleConfig;

        //}

        public AlertRules(ILogger logger, ICloudAppConfig appconfig)
        {
            _logger = logger;
            _appconfig = appconfig;
        }

        public AlertRuleResource CreateAlertRule(IResourceGroup resgrp, string targetResUriWebSite)
        {
            using (var imc = new InsightsManagementClient(_appconfig.Creds) { SubscriptionId = _appconfig.SubscriptionId })
            {

                _logger.Info("CreateAlertRule - Creating");

                var alertRuleRes = CreateAlertRuleFor500Errors(targetResUriWebSite, resgrp);
                var x = imc.AlertRules.CreateOrUpdate(resgrp.Name, alertRuleRes.Name, alertRuleRes);
                _logger.Info("CreateAlertRule - Created");

                return x;

            }
        }

        private AlertRuleResource CreateAlertRuleFor500Errors(string resUri, IResourceGroup resGrpName)
        {
            _logger.Info("CreateAlertRuleFor500Errors - Create");
            var alertRuleConfig = (IAlertRuleConfiguration)_appconfig;
            var actions = ConfigureRuleAction(alertRuleConfig);
            var conditions = ConfigureRuleThresholds(resUri);

            var alertRuleResource = new AlertRuleResource(
                                                          name: alertRuleConfig.RuleName,
                                                          description: "500 Errors Thrown",
                                                          location: "eastus2",
                                                          alertRuleResourceName: resGrpName.Name,
                                                          actions: actions,
                                                          condition: conditions,
                                                          isEnabled: true,
                                                          lastUpdatedTime: DateTime.Now
                                                         );
            return alertRuleResource;


        }

        private ThresholdRuleCondition ConfigureRuleThresholds(string resUri)
        {
            _logger.Info("ThresholdRuleCondition - Create");

            return new ThresholdRuleCondition()
            {
                DataSource = new RuleMetricDataSource()
                {
                    MetricName = "Http5xx",
                    ResourceUri = resUri
                },
                WindowSize = TimeSpan.FromMinutes(5),
                OperatorProperty = ConditionOperator.GreaterThan,
                Threshold = 1
            };
        }

        private List<RuleAction> ConfigureRuleAction(IAlertRuleConfiguration ruleConfig)
        {
            _logger.Info("RuleAction - Create");
            return new List<RuleAction>
            {
                new RuleEmailAction()
                {
                    CustomEmails = ruleConfig.RuleAlertMailAddress,
                    SendToServiceOwners = true
                }
            };

        }

    }
}