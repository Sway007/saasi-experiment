using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Business_microservice.Controllers
{
    public class BusinessController : Controller
    {
        private Configuration ConfigSettings { get; set; }
        // GET: /<controller>/
        public BusinessController(IOptions<Configuration> settings)
        {
            ConfigSettings = settings.Value;
        }
        //public IActionResult Index(Guid guid, DateTime timestart, int? io = 0, int? cpu = 0, int? memory = 0, int timetorun = 0, int id = 0, int timeout = 0)
        public IActionResult Index(Guid guid,String timestart, int? io = 0, int? cpu = 0, int? memory = 0, int timetorun = 0, int id = 0, int timeout = 0)
        {
            
            var factory = new ConnectionFactory() { HostName = "rabbitmq" };
            //var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "mono", type: "direct");
                string message = Convert.ToString(io) + " " + Convert.ToString(cpu) + " " + Convert.ToString(memory) + " " + Convert.ToString(timetorun) + " " +  Convert.ToString(timeout) + " " + timestart;
                Console.WriteLine(message);
                var body = Encoding.UTF8.GetBytes(message);
                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;
                channel.BasicPublish(exchange: "mono",
                                      routingKey: "business",
                                      basicProperties: properties,
                                      body: body);


             //   DateTime completeRunTime = System.DateTime.Now;
                // if (startRunTime.AddSeconds(timeout).CompareTo(completeRunTime) > 0) //check timeout
            /*    Console.WriteLine("send to DM");
                var message2 = Convert.ToString(id) + " " + guid;
                var body2 = Encoding.UTF8.GetBytes(message2);
                channel.BasicPublish(exchange: "call",
                      routingKey: "report",
                      basicProperties: null,
                      body: body2);
                      */
                return View();
            }
        }


    }
}
