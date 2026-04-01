using System;
using System.IO;
using CwcuClientTestApp.Core;

namespace CwcuClientTestApp.Infrastructure
{
    public class FileLogger : ILogger
    {
        private readonly string _logPath;

        public FileLogger(string logPath = "modbus_actions.log")
        {
            _logPath = logPath;
        }

        public void Log(string message)
        {
            Write($"[INFO] {message}");
        }

        public void LogError(string message)
        {
            Write($"[ERROR] {message}");
        }

        private void Write(string message)
        {
            string formattedMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] {message}";
            Console.WriteLine(formattedMessage);
            try
            {
                File.AppendAllText(_logPath, formattedMessage + Environment.NewLine);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Logging Failed] {ex.Message}");
            }
        }
    }
}
