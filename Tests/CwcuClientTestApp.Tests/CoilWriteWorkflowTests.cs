using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using CwcuClientTestApp.Core;
using CwcuClientTestApp.Application;

namespace CwcuClientTestApp.Tests
{
    [TestFixture]
    public class CoilWriteWorkflowTests
    {
        [Test]
        public async Task ExecuteAsync_Should_ExecuteWorkflow_Correctly()
        {
            // Arrange
            var config = new AppConfig
            {
                DeviceIp = "192.168.1.1",
                DevicePort = 502,
                SettlingDelayMs = 10, // Small delay for testing
                SlaveId = 1,
                Commands = new List<ModbusCommand>
                {
                    new ModbusCommand { CoilAddress = 5, Value = true },
                    new ModbusCommand { CoilAddress = 10, Value = false }
                }
            };

            var mockConfigProvider = new Mock<IConfigProvider>();
            mockConfigProvider.Setup(cp => cp.GetConfig()).Returns(config);

            var mockModbusClient = new Mock<IModbusClient>();
            mockModbusClient.Setup(m => m.ConnectAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                            .ReturnsAsync(true);

            var mockLogger = new Mock<ILogger>();

            var workflow = new CoilWriteWorkflow(mockConfigProvider.Object, mockModbusClient.Object, mockLogger.Object);

            // Act
            await workflow.ExecuteAsync();

            // Assert
            mockModbusClient.Verify(m => m.ConnectAsync(config.DeviceIp, config.DevicePort, It.IsAny<CancellationToken>()), Times.Once);

            mockModbusClient.Verify(m => m.WriteSingleCoilAsync(config.SlaveId, 5, true), Times.Once);
            mockModbusClient.Verify(m => m.WriteSingleCoilAsync(config.SlaveId, 10, false), Times.Once);

            mockModbusClient.Verify(m => m.Disconnect(), Times.Once);
        }

        [Test]
        public async Task ExecuteAsync_Should_Handle_ConnectionFailure()
        {
            // Arrange
            var mockConfigProvider = new Mock<IConfigProvider>();
            mockConfigProvider.Setup(cp => cp.GetConfig()).Returns(new AppConfig());

            var mockModbusClient = new Mock<IModbusClient>();
            mockModbusClient.Setup(m => m.ConnectAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                            .ThrowsAsync(new System.Exception("Connection failed"));

            var mockLogger = new Mock<ILogger>();

            var workflow = new CoilWriteWorkflow(mockConfigProvider.Object, mockModbusClient.Object, mockLogger.Object);

            // Act
            await workflow.ExecuteAsync();

            // Assert
            mockModbusClient.Verify(m => m.ConnectAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
            
            // It should NOT attempt to write coils if connection fails
            mockModbusClient.Verify(m => m.WriteSingleCoilAsync(It.IsAny<byte>(), It.IsAny<ushort>(), It.IsAny<bool>()), Times.Never);
            mockLogger.Verify(l => l.LogError(It.Is<string>(s => s.Contains("Connection failed"))), Times.Once);
        }
    }
}
