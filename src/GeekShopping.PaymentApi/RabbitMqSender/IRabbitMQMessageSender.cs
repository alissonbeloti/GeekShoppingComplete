using GeekShopping.MessageBus;

namespace GeekShopping.PaymentApi.RabbitMqSender
{
    public interface IRabbitMQMessageSender
    {
        void SendMessage(BaseMessage baseMessage);
    }
}
