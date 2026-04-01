using System.Threading.Tasks;

namespace ModbusTcpClientAutomation.Core
{
    public interface IAutomationWorkflow
    {
        Task ExecuteAsync();
    }
}
