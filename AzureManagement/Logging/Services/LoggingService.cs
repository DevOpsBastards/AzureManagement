using System;
using System.Reflection;
using NLog;
using NLog.LayoutRenderers;
using ILogger = AzureManagement.Logging.Interfaces.ILogger;

namespace AzureManagement.Logging.Services
{
    /// <summary>
    ///     Log implementation uses nlog
    /// </summary>
    /// <example>
    ///     private static readonly ILogger Logger = new LoggingService();
    ///     public static void Main(string[] args)
    ///     {
    ///     logger.Info("Program startup");
    ///     }
    /// </example>
    public class LoggingService : ILogger
    {
        private static Func<Logger> _logger;
        private AppSettingLayoutRenderer _l = new NLog.LayoutRenderers.AppSettingLayoutRenderer(); //force nlog.extended to move to bin
        //private AspApplicationValueLayoutRenderer _aspnet = new AspApplicationValueLayoutRenderer(); //force nlog.web to move to bin

        public LoggingService(string appName = "")
        {
            appName = GetAppName(appName);
            
            _logger = () => LogManager.GetLogger(appName);
        }

        public string Name => _logger().Name;

        public virtual void Error(string message)
        {
            _logger().Error(message);
        }

        public virtual void Error(string message, Exception ex)
        {
            _logger().Error(ex, message);
        }

        public virtual void Info(string message)
        {
            _logger().Info(message);
        }

        private string GetAppName(string appName)
        {
            if (string.IsNullOrEmpty(appName))
            {
                appName = Assembly.GetEntryAssembly() != null
                              ? Assembly.GetEntryAssembly().GetName().Name //Needed for calling assembly
                              : GetType().Assembly.GetName(false).Name; //needed for unit test calls
            }

            return appName;
        }
    }
}
