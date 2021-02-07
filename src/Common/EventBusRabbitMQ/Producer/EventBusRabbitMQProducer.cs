using EventBusRabbitMQ.Events;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventBusRabbitMQ.Producer
{
    public class EventBusRabbitMQProducer
    {
        private readonly IRabbitMQConnection _connection;

        public EventBusRabbitMQProducer(IRabbitMQConnection connection)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        public void PublishBasketCheckout(string queueName, BasketCheckoutEvent publishModel)
        {
            using var chanel = _connection.CreateModel();
            chanel.QueueDeclare(queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            string message = JsonConvert.SerializeObject(publishModel);
            byte[] body = Encoding.UTF8.GetBytes(message);

            IBasicProperties properties = chanel.CreateBasicProperties();
            properties.Persistent = true;
            properties.DeliveryMode = 2;

            chanel.ConfirmSelect();
            chanel.BasicPublish(exchange: "", routingKey: queueName, mandatory: true, basicProperties: properties, body: body);
            chanel.WaitForConfirmsOrDie();

            chanel.BasicAcks += (sender, eventArgs) =>
            {
                Console.WriteLine("Send RabbitMQ");
            };
            chanel.ConfirmSelect();
        }
    }
}
