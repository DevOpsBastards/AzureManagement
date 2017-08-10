namespace AzureManagement
{
    public class EnvironmentType : Enumeration
    {
        public EnvironmentType() { }

        protected EnvironmentType(int value, string displayName) : base(value, displayName) { }

        public static readonly EnvironmentType Test = new EnvironmentType(10, "Test");

        public static readonly EnvironmentType Prod = new EnvironmentType(80, "Prod");

        public static readonly EnvironmentType Unknown = new EnvironmentType(99, "Unknown");

    }



}
