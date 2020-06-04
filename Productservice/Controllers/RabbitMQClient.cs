using Productservice_2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;


namespace Productservice_2.Controllers
{
    internal class RabbitMQClient
    {

        private readonly ILogger _logger;
        private IConnection _connection;
        private IModel _channel;
        public RabbitMQClient()
        {
            //this._logger = loggerFactory.CreateLogger<ProductService>();
            configureRabbit();
        }
        public void configureRabbit()
        {
            //Step1
            var factory = new ConnectionFactory { HostName = "localhost" };

            //Step2
            _connection = factory.CreateConnection();

            //Step3
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare("ex13", ExchangeType.Topic);
            _channel.QueueDeclare("queue13", false, false, false, null);
            _channel.QueueBind("queue13", "ex13", "ss.*");
            _channel.BasicQos(0, 1, false);
           // _connection.ConnectionShutdown += RabbitMQ_connectionshutdown;
        }

        internal void sendDetail(Product product)
        {
            byte[] msgbody = System.Text.Encoding.UTF8.GetBytes(product.ToString());
            sendMessage(msgbody, "ss.*");
            Console.WriteLine("Message sent");   
        }

        private void sendMessage(byte[] msgbody, string v)
        {
            _channel.BasicPublish("ex13", v, null, msgbody);
        }

        internal void close()
        {
            _connection.Close();
        }
        
    }
}