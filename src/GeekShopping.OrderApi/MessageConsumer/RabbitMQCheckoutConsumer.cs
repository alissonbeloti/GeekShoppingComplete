using GeekShopping.OrderApi.Messages;
using GeekShopping.OrderApi.Model;
using GeekShopping.OrderApi.RabbitMqSender;
using GeekShopping.OrderApi.Repository;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

using System.Text;
using System.Text.Json;
using System.Threading.Channels;

namespace GeekShopping.OrderApi.MessageConsumer;

public class RabbitMQCheckoutConsumer : BackgroundService
{
    private readonly OrderRepository _repository;
    private IConnection _connection;
    private IModel _channel;
    private IRabbitMQMessageSender _messageSender;

    public RabbitMQCheckoutConsumer(OrderRepository repository, IRabbitMQMessageSender messageSender)
    {
        _repository = repository;
        _messageSender = messageSender;

        var factory = new ConnectionFactory
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest",
        };
        _connection = factory.CreateConnection();

        _channel = _connection.CreateModel();
        _channel.QueueDeclare("checkoutqueue", false, false, false);

    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (channel, evt) =>
        {
            var content = Encoding.UTF8.GetString(evt.Body.ToArray());
            CheckoutHeaderVO vo = JsonSerializer.Deserialize<CheckoutHeaderVO>(content) ?? new CheckoutHeaderVO();
            ProcessOrder(vo).GetAwaiter().GetResult();
            _channel.BasicAck(evt.DeliveryTag, false);
        };
        _channel.BasicConsume("checkoutqueue", false, consumer);
        return Task.CompletedTask;
    }

    private async Task ProcessOrder(CheckoutHeaderVO vo)
    {
        OrderHeader order = new OrderHeader { 
            CardNumber = vo.CardNumber,
            CartDetails = new List<OrderDetail>(),
            UserId = vo.UserId,
            FirstName = vo.FirstName,
            LastName = vo.LastName,
            CartTotalItens = vo.CartTotalItens,
            CouponCode = vo.CouponCode,
            Cvv = vo.Cvv,
            DateTime = vo.DateTime.HasValue? new DateOnly(vo.DateTime.Value.Year, vo.DateTime.Value.Month, vo.DateTime.Value.Day): null,
            DiscountAmount = vo.DiscountAmount,
            Email = vo.Email,
            ExpireMonthYear = vo.ExpireMonthYear,
            Id = 0,
            PaymentStatus = false,
            Phone = vo.Phone,
            PurchaseAmount = vo.PurchaseAmount,
            PurchaseTime = vo.DateTime.HasValue? new TimeOnly(vo.DateTime.Value.Hour, vo.DateTime.Value.Minute, vo.DateTime.Value.Second, vo.DateTime.Value.Millisecond): null,
        };

        foreach (var detail in vo.CartDetails)
        {
            OrderDetail od = new ()
            {
                ProductId = detail.ProductId,
                Count = detail.Count,
                ProductName = detail.Product.Name,
                Price = detail.Product.Price,
            };
            order.CartTotalItens += detail.Count;
            order.CartDetails.Add(od);
        }

        await _repository.AddOrder(order);

        PaymentVO payment = new()
        {
            Name = order.FirstName + " " + order.LastName,
            CardNumber = order.CardNumber,
            Cvv = order.Cvv,
            Email = order.Email,
            ExpireMonthYear= order.ExpireMonthYear,
            Id=0,
            MessageCreated = DateTime.Now,
            OrderId = order.Id,
            PurchaseAmount = order.PurchaseAmount,
        };

        try
        {
            _messageSender.SendMessage(payment, "orderpaymentprocessqueue");
        }
        catch (Exception)
        {
            //Log
            throw;
        }
    }
}
