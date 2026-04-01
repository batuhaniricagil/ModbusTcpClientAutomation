using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using CwcuClientTestApp.Core;
using NModbus;

namespace CwcuClientTestApp.Infrastructure
{
    public class NModbusClient : IModbusClient, IDisposable
    {
        private TcpClient _tcpClient;
        private IModbusMaster _master;
        private bool _disposed;

        public async Task<bool> ConnectAsync(string ipAddress, int port, CancellationToken cancellationToken = default)
        {
            _tcpClient = new TcpClient();
            try
            {
                await _tcpClient.ConnectAsync(ipAddress, port, cancellationToken);
                var factory = new ModbusFactory();
                _master = factory.CreateMaster(_tcpClient);
                return true;
            }
            catch (Exception)
            {
                Disconnect();
                throw;
            }
        }

        public async Task WriteSingleCoilAsync(byte slaveId, ushort coilAddress, bool value)
        {
            if (_master == null)
                throw new InvalidOperationException("Not connected to Modbus device.");

            await _master.WriteSingleCoilAsync(slaveId, coilAddress, value);
        }

        public void Disconnect()
        {
            if (_tcpClient != null)
            {
                _tcpClient.Close();
                _tcpClient.Dispose();
                _tcpClient = null;
            }
            _master = null;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                Disconnect();
                _disposed = true;
            }
        }
    }
}
