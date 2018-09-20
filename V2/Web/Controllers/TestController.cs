using Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Microsoft.ServiceFabric.Services.Remoting.FabricTransport;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: FabricTransportServiceRemotingProvider(MaxMessageSize = 134217728)]
namespace Web.Controllers
{
    public class TestController : Controller
    {
        [HttpGet]
        public List<Fixture> Getfixtures()
        {
            List<Fixture> fixtureList = null;
            //return Task.FromResult("hello");
            StringBuilder builder = new StringBuilder();
            IMyService helloWorldClient = null;
            try
            {
                var proxyFactory = new ServiceProxyFactory((c) =>
                {
                    return new FabricTransportServiceRemotingClientFactory();
                });

                helloWorldClient = proxyFactory.CreateServiceProxy<IMyService>(new Uri("fabric:/PingSF/Stateful"), new ServicePartitionKey(0));
                fixtureList = helloWorldClient.GetFixtureList().GetAwaiter().GetResult();
            }
            catch (Exception e)
            {
                builder.AppendLine().Append("error from statefull service").Append(e.Message);
            }

            return fixtureList;
        }

        [HttpGet]
        public string Gettest()
        {
            return "hello world";
        }
    }
}
