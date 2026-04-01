using NUnit.Framework;
using ModbusTcpClientAutomation.Config;

namespace ModbusTcpClientAutomation.Tests.Config
{
    [TestFixture]
    public class AppConfigTests
    {
        [Test]
        public void AppConfig_DefaultConstructor_ShouldSetDefaultValues()
        {
            // Arrange & Act
            var config = new AppConfig();

            // Assert
            Assert.That(config.DeviceIp, Is.EqualTo("127.0.0.1"));
            Assert.That(config.DevicePort, Is.EqualTo(502));
            Assert.That(config.SettlingDelayMs, Is.EqualTo(1000));
            Assert.That(config.SlaveId, Is.EqualTo(1));
            Assert.That(config.Commands, Is.Not.Null);
            Assert.That(config.Commands.Count, Is.EqualTo(2));
            Assert.That(config.Commands[0].CoilAddress, Is.EqualTo(0));
            Assert.That(config.Commands[0].Value, Is.True);
            Assert.That(config.Commands[1].CoilAddress, Is.EqualTo(1));
            Assert.That(config.Commands[1].Value, Is.False);
        }
    }
}
