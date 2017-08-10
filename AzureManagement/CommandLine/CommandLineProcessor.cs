using System;
using AzureManagement.Logging.Interfaces;

[assembly: CLSCompliant(false)]

namespace AzureManagement.CommandLine
{
    /// <summary>
    /// Convert command line args into type safe properties
    /// </summary>
    /// <example>
    ///   var clp = new CommandLineProcessor(args, logger);
    ///   Console.WriteLine(clp.Options.IsDebug);
    /// </example>

    public class CommandLineProcessor
    {
        private readonly ILogger _logger;
        public CommandArgumentOptions Options { get; private set; }

        public CommandLineProcessor(string[] args, ILogger log)
        {
            _logger = log ?? throw new ArgumentNullException(nameof(log), "ILogger cannot be null");

            _logger.Info("Starting CommandLineProcessor");

            ProcessCommandLine(args);
        }

        private void ProcessCommandLine(string[] args)
        {
            _logger.Info("Setting command argument options");
            Options = new CommandArgumentOptions();

            if ((args != null) && (args.Length != 0))
            {
                global::CommandLine.Parser.Default.ParseArguments(args, Options);
                //if (CommandLine.Parser.Default.ParseArguments(args, Options))
                //{
                // Values are available here
                //if (options.Verbose)
                //   return string.Format("Debug: {0}", options.IsDebug);
                //}

                _logger.Info("Args found and set");
                return;
            }

            _logger.Info("No options set return help");
            //Options.GetUsage();


            //return Options.GetUsage();

        }
    }
}
