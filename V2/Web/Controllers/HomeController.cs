using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Fabric;
using System.Fabric.Query;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Microsoft.ServiceFabric.Services.Remoting.FabricTransport;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Client;
using Web.Models;

namespace Web.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {

            StringBuilder builder = new StringBuilder();

            IMyService2 helloWorldClient2 = null;
            string res = null;
            try
            {

                var proxyFactory = new ServiceProxyFactory((c) =>
                {
                    var settings = new FabricTransportRemotingSettings();
                    settings.UseWrappedMessage = true;
                    return new FabricTransportServiceRemotingClientFactory(settings);
                });

                helloWorldClient2 = proxyFactory.CreateServiceProxy<IMyService2>(new Uri("fabric:/PingSF/Stateless"));
                res = helloWorldClient2.GetFixtureList().GetAwaiter().GetResult();
                builder.Append(res);
            }
            catch (Exception e)
            {
                builder.AppendLine().Append("error from statefull service").Append(e.Message);
            }

            ViewData["Message"] = builder;
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
