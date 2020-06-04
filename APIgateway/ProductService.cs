using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace APIgateway
{
    public class ProductService:BackgroundService
    {

        private readonly ILogger _logger;
        private IConnection _connection;
        private IModel _channel;
        public ProductService(ILoggerFactory loggerFactory)
        {
            this._logger = loggerFactory.CreateLogger<ProductService>();
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
            _channel.QueueDeclare("queue13",false,false,false,null);
            _channel.QueueBind("queue13","ex13","ss.*");
            _channel.BasicQos(0, 1, false);
            _connection.ConnectionShutdown += RabbitMQ_connectionshutdown;
        }

        protected override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            //Step4
            var consumer = new EventingBasicConsumer(_channel);

            //Step5
            consumer.Received += (ch, ea) =>
             {
                 var content = System.Text.Encoding.UTF8.GetString(ea.Body.ToArray());
                 HandleMessage(content);
                 _channel.BasicAck(ea.DeliveryTag, false);
             };
            _channel.BasicConsume("queue13", false, consumer);
            return Task.CompletedTask;
        }

        private void HandleMessage(string content)
        {
           _logger.LogInformation($"consumer received message {content}");
        }

        private void RabbitMQ_connectionshutdown(object sender, ShutdownEventArgs e)
        {
            _logger.LogInformation($"connection is shut down {e.ReplyText}");
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}
