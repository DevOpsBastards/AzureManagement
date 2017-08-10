using System.Collections.Generic;

namespace AzureManagement
{
    public class WebApp
    {
        public string Name { get; set; }
        public string ResourceGroupName { get; set; }
        public ISet<string> Urls { get; set; }

        public WebApp()
        {
            Urls = new HashSet<string>();
        }

        public override string ToString()
        {
            return $"{Name}|{string.Join(",", Urls)}";
        }
    }
}
