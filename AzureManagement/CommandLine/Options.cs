using System;
using CommandLine;
using CommandLine.Text;

namespace AzureManagement.CommandLine
{
    public class CommandArgumentOptions
    {

        // Omitting long name, default --verbose
        [Option("v", HelpText = "Prints all messages to standard output.")]
        public bool Verbose { get; set; }

        //--d
        [Option('d', "debug", Required = false, HelpText = "Turns debugging mode on.")]
        public bool IsDebug { get; set; }

        [Option('o', "outputFile", Required = false, HelpText = "output file")]
        public string OutFilePath { get; set; }

        [Option("Action", Required = true, HelpText = "Environment to configure")]
        public string Action { get; set; }


        [Option('e', "envtype", Required = true, HelpText = "Environment to configure")]
        public string EnvironmentName { get; set; }

        [Option('t', "apptype", Required = true, HelpText = "Application type to configure")]
        public string ApplicationTypeName { get; set; }

        [Option('a', "appname", Required = true, HelpText = "Application base Name to configure")]
        public string ApplicationBaseName { get; set; }

        [Option("companyabbrv", Required = false, HelpText = "Company Abbrev Prefix used to name everything")]
        public string CompanyAbbrevPrefix { get; set; }

        [Option("scaleemail", Required = true, HelpText = "email to use when auto-scale occurs")]
        public string AutoScaleNotificationEmails { get; set; }

        [Option("alertemail", Required = true, HelpText = "email to use when alerts occurs")]
        public string RuleAlertEmails { get; set; }



        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        [HelpOption(HelpText = "Display this help screen.")]
        public string GetUsage()
        {

            var help = new HelpText
            {
                Heading = new HeadingInfo(
                    $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name} Version: {System.Reflection.Assembly.GetExecutingAssembly().GetName().Version}"),
                Copyright = new CopyrightInfo(" ", System.DateTime.Now.Year),
                AdditionalNewLineAfterOption = true,
                AddDashesToOption = true
            };
            help.AddPreOptionsLine(
                $"Usage: {System.Reflection.Assembly.GetExecutingAssembly().GetName().Name} site action");
            help.AddOptions(this);
            return help;

        }
    }
}
