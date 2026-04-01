using System.IO;
using System.Text.Json;
using Moq;
using NUnit.Framework;
using ModbusTcpClientAutomation.Interfaces;
using ModbusTcpClientAutomation.Config;
using ModbusTcpClientAutomation.Infrastructure;

namespace ModbusTcpClientAutomation.Tests.Infrastructure
{
    [TestFixture]
    public class JsonConfigProviderTests
    {
        [Test]
        public void GetConfig_WhenFileExists_ShouldReturnConfig()
        {
            // Arrange
            string testFile = "test_settings_exist.json";
            var expectedConfig = new AppConfig { DeviceIp = "10.0.0.1", DevicePort = 1234 };
            File.WriteAllText(testFile, JsonSerializer.Serialize(expectedConfig));

            var mockLogger = new Mock<ILogger>();
            var provider = new JsonConfigProvider(mockLogger.Object, testFile);

            // Act
            var config = provider.GetConfig();

            // Assert
            Assert.That(config, Is.Not.Null);
            Assert.That(config.DeviceIp, Is.EqualTo("10.0.0.1"));
            Assert.That(config.DevicePort, Is.EqualTo(1234));

            // Cleanup
            if (File.Exists(testFile)) File.Delete(testFile);
        }

        [Test]
        public void GetConfig_WhenFileMissing_ShouldCreateDefaultFileAndReturnDefaultConfig()
        {
            // Arrange
            string testFile = "test_settings_missing.json";
            if (File.Exists(testFile)) File.Delete(testFile);

            var mockLogger = new Mock<ILogger>();
            var provider = new JsonConfigProvider(mockLogger.Object, testFile);

            // Act
            var config = provider.GetConfig();

            // Assert
            Assert.That(File.Exists(testFile), Is.True);
            var defaultConfig = new AppConfig();
            Assert.That(config.DeviceIp, Is.EqualTo(defaultConfig.DeviceIp));
            mockLogger.Verify(l => l.Log(It.Is<string>(s => s.Contains("Created default config file"))), Times.Once);

            // Cleanup
            if (File.Exists(testFile)) File.Delete(testFile);
        }
    }
}
