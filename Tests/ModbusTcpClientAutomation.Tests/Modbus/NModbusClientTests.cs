using System;
using System.Threading.Tasks;
using NUnit.Framework;
using ModbusTcpClientAutomation.Modbus;

namespace ModbusTcpClientAutomation.Tests.Modbus
{
    [TestFixture]
    public class NModbusClientTests
    {
        [Test]
        public void WriteSingleCoilAsync_WhenNotConnected_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var client = new NModbusClient();

            // Act & Assert
            var ex = Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await client.WriteSingleCoilAsync(1, 0, true);
            });

            Assert.That(ex.Message, Does.Contain("Not connected"));
        }

        [Test]
        public void Disconnect_WhenNotConnected_ShouldNotThrow()
        {
            // Arrange
            var client = new NModbusClient();

            // Act & Assert
            Assert.DoesNotThrow(() => client.Disconnect());
        }

        [Test]
        public void Dispose_ShouldNotThrow()
        {
            // Arrange
            var client = new NModbusClient();

            // Act & Assert
            Assert.DoesNotThrow(() => client.Dispose());
        }
    }
}
