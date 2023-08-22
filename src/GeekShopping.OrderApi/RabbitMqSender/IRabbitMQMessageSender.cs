using GeekShopping.MessageBus;

namespace GeekShopping.OrderApi.RabbitMqSender
{
    public interface IRabbitMQMessageSender
    {
        void SendMessage(BaseMessage baseMessage, string queueName);
    }
}
