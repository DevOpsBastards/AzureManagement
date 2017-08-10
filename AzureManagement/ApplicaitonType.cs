namespace AzureManagement
{

    public class ApplicationType : Enumeration
    {
        protected ApplicationType(int value, string displayName) : base(value, displayName) { }

        public ApplicationType() { }

        public static readonly ApplicationType Web = new ApplicationType(10, "Web");

        public static readonly ApplicationType Svc = new ApplicationType(20, "Svc");

        public static readonly ApplicationType Job = new ApplicationType(30, "Job");

        public static readonly ApplicationType Cache = new ApplicationType(40, "Cache");

        public static readonly ApplicationType Db = new ApplicationType(50, "Db");

        public static readonly ApplicationType Unknown = new ApplicationType(99, "Unknown");

    }

}
