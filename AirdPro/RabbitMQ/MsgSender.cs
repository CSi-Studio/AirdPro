using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AirdPro.RabbitMQ
{
    class MsgSender
    {
        private static MsgSender instance;
        private ConnectionFactory factory;
        private IConnection connection;
        private IModel channel;
        private MsgSender() 
        {
            factory = new ConnectionFactory();
            factory.RequestedConnectionTimeout = TimeSpan.FromSeconds(30);
            factory.Uri = new Uri("amqp://airdpro:qazse@192.168.31.78:5672");
            try
            {
                connection = factory.CreateConnection();
                channel = connection.CreateModel();
            }catch(Exception e)
            {
                throw e;
            }
        }

        public static MsgSender getInstance()
        {
            if(instance == null)
            {
                instance = new MsgSender();
            }
            return instance;
        }


        public void send(String content)
        {
           channel.QueueDeclare("LIMS", false, false, false, null);//创建一个名称为hello的消息队列
           var body = Encoding.UTF8.GetBytes(content);
           channel.BasicPublish("", "hello", null, body); //开始传递
        }

        public void receive()
        {
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (ch, ea) =>
            {
                var body = ea.Body;
                Console.Out.WriteLine(body);
                channel.BasicAck(ea.DeliveryTag, false);
            };
            String consumerTag = channel.BasicConsume("LIMS", false, consumer);
        }
    }
}
