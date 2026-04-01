using System.Threading;
using System.Threading.Tasks;

namespace ModbusTcpClientAutomation.Core
{
    public interface IModbusClient
    {
        Task<bool> ConnectAsync(string ipAddress, int port, CancellationToken cancellationToken = default);
        Task WriteSingleCoilAsync(byte slaveId, ushort coilAddress, bool value);
        void Disconnect();
    }
}
