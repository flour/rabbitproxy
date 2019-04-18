using EasyNetQ;
using Microsoft.Extensions.Configuration;
using Rabbitproxy.Common;
using System;
using System.Threading.Tasks;

namespace Rabbitproxy.Services
{
    public class MessageBusRabbitMQ : IMessageBus, IDisposable
    {
        private bool disposed = false;
        private IBus _msgBus;
        private readonly string rabbitHost;

        public MessageBusRabbitMQ(IConfiguration Configuration)
        {

            rabbitHost = Configuration.GetValue<string>(Constants.RABBIT_HOST_KEY);
            Connect();
        }

        public void Connect()
        {
            _msgBus = RabbitHutch.CreateBus($"host={rabbitHost}");
        }

        public async Task<Shared.IMessage> HandleMessageAsync(Shared.IMessage message)
        {
            try
            {
                var response = await _msgBus.RequestAsync<Shared.IMessage, Shared.IMessage>(message);
                return response;
            }
            catch (Exception ex)
            {
                return new Shared.MessageResponse() { Message = ex.Message };
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if (!disposed && disposing)
            {
                _msgBus.Dispose();
                disposed = true;
            }
        }

        ~MessageBusRabbitMQ()
        {
            Dispose(false);
        }
    }
}
