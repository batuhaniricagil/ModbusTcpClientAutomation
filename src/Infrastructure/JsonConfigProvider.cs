using System;
using System.IO;
using System.Text.Json;
using ModbusTcpClientAutomation.Interfaces;
using ModbusTcpClientAutomation.Config;

namespace ModbusTcpClientAutomation.Infrastructure
{
    public class JsonConfigProvider : IConfigProvider
    {
        private readonly string _configPath;
        private readonly ILogger _logger;

        public JsonConfigProvider(ILogger logger, string configPath = "Config/appsettings.json")
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            
            // Resolve path relative to the application's base directory if not absolute
            _configPath = Path.IsPathRooted(configPath) 
                ? configPath 
                : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configPath);
        }

        public AppConfig GetConfig()
        {
            if (File.Exists(_configPath))
            {
                try
                {
                    string json = File.ReadAllText(_configPath);
                    return JsonSerializer.Deserialize<AppConfig>(json) ?? new AppConfig();
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error reading {_configPath}: {ex.Message}");
                    return new AppConfig();
                }
            }
            else
            {
                var config = new AppConfig();
                try
                {
                    File.WriteAllText(_configPath, JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true }));
                    _logger.Log($"Created default config file at {_configPath}. Please configure it and run again.");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Failed to write default config: {ex.Message}");
                }
                return config;
            }
        }
    }
}
