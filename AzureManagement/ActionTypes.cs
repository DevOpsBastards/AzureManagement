namespace AzureManagement
{
    public class ActionTypes : Enumeration
    {
        protected ActionTypes(int value, string displayName) : base(value, displayName) { }

        public ActionTypes() { }

        public static readonly ActionTypes CreateResGroup = new ActionTypes(10, "RSG");

        public static readonly ActionTypes CreateAppPlan = new ActionTypes(20, "APPPLAN");

        public static readonly ActionTypes CreateWebApp = new ActionTypes(40, "WEBAPP");

        public static readonly ActionTypes Unknown = new ActionTypes(99, "Unknown");
    }


}
