using Shared;
using System.Threading.Tasks;

namespace Rabbitproxy.Services
{
    public interface IMessageBus
    {
        void Connect();
        Task<IMessage> HandleMessageAsync(IMessage message);
    }
}
