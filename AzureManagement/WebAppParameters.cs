namespace AzureManagement
{
    public class WebAppParameters
    {
        public string Schema { get; set; }
        public string ContentVersion { get; set; }
        public Parameters Parameters { get; set; }
    }

    public class Parameters
    {
        public AlertrulesScaleupName AlertrulesScaleupName { get; set; }
        public Alertrules500Name Alertrules500Name { get; set; }
        public AutoscalesettingsScaleoutName AutoscalesettingsScaleoutName { get; set; }
        public ComponentsName ComponentsName { get; set; }
        public ServerfarmsServiceplanName ServerfarmsServicePlanName { get; set; }
        public SitesName SitesName { get; set; }
        public ConfigWebName ConfigWebName { get; set; }
    }

    public class AlertrulesScaleupName
    {
        public object Value { get; set; }
    }

    public class Alertrules500Name
    {
        public object Value { get; set; }
    }

    public class AutoscalesettingsScaleoutName
    {
        public object Value { get; set; }
    }

    public class ComponentsName
    {
        public object Value { get; set; }
    }

    public class ServerfarmsServiceplanName
    {
        public object Value { get; set; }
    }

    public class SitesName
    {
        public object Value { get; set; }
    }

    public class ConfigWebName
    {
        public object Value { get; set; }
    }
}