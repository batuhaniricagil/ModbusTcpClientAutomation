using System;
using System.Threading;
using System.Threading.Tasks;
using ModbusTcpClientAutomation.Interfaces;
using ModbusTcpClientAutomation.Config;

namespace ModbusTcpClientAutomation.Application
{
    public class CoilWriteWorkflow : IAutomationWorkflow
    {
        private readonly IConfigProvider _configProvider;
        private readonly IModbusClient _modbusClient;
        private readonly ILogger _logger;

        public CoilWriteWorkflow(IConfigProvider configProvider, IModbusClient modbusClient, ILogger logger)
        {
            _configProvider = configProvider ?? throw new ArgumentNullException(nameof(configProvider));
            _modbusClient = modbusClient ?? throw new ArgumentNullException(nameof(modbusClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task ExecuteAsync()
        {
            var config = _configProvider.GetConfig();

            _logger.Log($"Starting Modbus Client Workflow. Target: {config.DeviceIp}:{config.DevicePort}");

            try
            {
                // Connect with a 5-second timeout
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
                try
                {
                    await _modbusClient.ConnectAsync(config.DeviceIp, config.DevicePort, cts.Token);
                }
                catch (OperationCanceledException)
                {
                    _logger.LogError("Connection timed out.");
                    return;
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Connection failed: {ex.Message}");
                    return;
                }

                if (config.Commands != null)
                {
                    foreach (var command in config.Commands)
                    {
                        try
                        {
                            string valStr = command.Value ? "TRUE" : "FALSE";
                            _logger.Log($"Writing Coil {command.CoilAddress} to {valStr}...");
                            await _modbusClient.WriteSingleCoilAsync(config.SlaveId, (ushort)command.CoilAddress, command.Value);
                            _logger.Log($"Success: Coil {command.CoilAddress} set to {valStr}. Device responded.");
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError($"Failed to write Coil {command.CoilAddress}: {ex.Message}");
                        }

                        // Wait settling delay
                        _logger.Log($"Waiting for command settling duration: {config.SettlingDelayMs} ms...");
                        await Task.Delay(config.SettlingDelayMs);
                    }
                }

                // Disconnect
                _logger.Log("Disconnecting from Modbus device.");
                _modbusClient.Disconnect();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error: {ex.Message}");
            }
            finally
            {
                _logger.Log("Execution completed.");
            }
        }
    }
}
