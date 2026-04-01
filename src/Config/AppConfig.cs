using System.Collections.Generic;

namespace ModbusTcpClientAutomation.Config
{
    public class ModbusCommand
    {
        public int CoilAddress { get; set; }
        public bool Value { get; set; }
    }

    public class AppConfig
    {
        public string DeviceIp { get; set; } = "127.0.0.1";
        public int DevicePort { get; set; } = 502;
        public int SettlingDelayMs { get; set; } = 1000;
        public byte SlaveId { get; set; } = 1;
        
        public List<ModbusCommand> Commands { get; set; } = new List<ModbusCommand>
        {
            new ModbusCommand { CoilAddress = 0, Value = true },
            new ModbusCommand { CoilAddress = 1, Value = false }
        };
    }
}
