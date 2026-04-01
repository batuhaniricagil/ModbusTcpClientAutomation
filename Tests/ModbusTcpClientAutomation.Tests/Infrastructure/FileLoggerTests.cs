using System;
using System.IO;
using NUnit.Framework;
using ModbusTcpClientAutomation.Infrastructure;

namespace ModbusTcpClientAutomation.Tests.Infrastructure
{
    [TestFixture]
    public class FileLoggerTests
    {
        private string _testLogPath;

        [SetUp]
        public void SetUp()
        {
            _testLogPath = Path.Combine(Path.GetTempPath(), $"test_log_{Guid.NewGuid()}.log");
        }

        [TearDown]
        public void TearDown()
        {
            if (File.Exists(_testLogPath))
            {
                File.Delete(_testLogPath);
            }
        }

        [Test]
        public void Log_ShouldWriteMessageToFile()
        {
            // Arrange
            var logger = new FileLogger(_testLogPath);
            string message = "Test info message";

            // Act
            logger.Log(message);

            // Assert
            Assert.That(File.Exists(_testLogPath), Is.True);
            string content = File.ReadAllText(_testLogPath);
            Assert.That(content, Does.Contain("[INFO]"));
            Assert.That(content, Does.Contain(message));
        }

        [Test]
        public void LogError_ShouldWriteErrorMessageToFile()
        {
            // Arrange
            var logger = new FileLogger(_testLogPath);
            string message = "Test error message";

            // Act
            logger.LogError(message);

            // Assert
            Assert.That(File.Exists(_testLogPath), Is.True);
            string content = File.ReadAllText(_testLogPath);
            Assert.That(content, Does.Contain("[ERROR]"));
            Assert.That(content, Does.Contain(message));
        }
    }
}
