using System;

namespace AzureManagement.Logging.Interfaces
{
    public interface ILogger
    {
        string Name { get; }
        void Info(string message);
        void Error(string message);
        void Error(string message, Exception ex);
    }
}