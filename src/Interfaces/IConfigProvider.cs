using ModbusTcpClientAutomation.Config;

namespace ModbusTcpClientAutomation.Interfaces
{
    public interface IConfigProvider
    {
        AppConfig GetConfig();
    }
}
