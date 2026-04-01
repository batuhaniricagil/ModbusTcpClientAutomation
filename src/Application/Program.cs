using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ModbusTcpClientAutomation.Interfaces;
using ModbusTcpClientAutomation.Infrastructure;
using ModbusTcpClientAutomation.Application;
using ModbusTcpClientAutomation.Config;
using ModbusTcpClientAutomation.Modbus;

namespace ModbusTcpClientAutomation.Application
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Setup Dependency Injection
            var services = new ServiceCollection();
            
            services.AddSingleton<ILogger, FileLogger>();
            services.AddSingleton<IConfigProvider, JsonConfigProvider>();
            services.AddTransient<IModbusClient, NModbusClient>();
            services.AddTransient<IAutomationWorkflow, CoilWriteWorkflow>();
            
            var serviceProvider = services.BuildServiceProvider();

            // Resolve the entry point workflow
            var workflow = serviceProvider.GetRequiredService<IAutomationWorkflow>();
            
            try
            {
                await workflow.ExecuteAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FATAL] Unhandled exception occurred: {ex.Message}");
            }
            
            if (serviceProvider is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}
