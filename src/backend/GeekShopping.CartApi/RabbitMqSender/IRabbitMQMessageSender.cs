using GeekShopping.MessageBus;

namespace GeekShopping.CartApi.RabbitMqSender
{
    public interface IRabbitMQMessageSender
    {
        void SendMessage(BaseMessage baseMessage, string queueName);
    }
}
