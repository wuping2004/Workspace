using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Fabric;
using System.Fabric.Query;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using Microsoft.ServiceFabric.Actors.Query;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Client;
using PingActor1.Interfaces;
using Web.Models;

namespace Web.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            StringBuilder builder = new StringBuilder();
            IMyService helloWorldClient = null;
            try
            {

                var proxyFactory = new ServiceProxyFactory((c) =>
                {   
                    return new FabricTransportServiceRemotingClientFactory();
                });

                helloWorldClient = proxyFactory.CreateServiceProxy<IMyService>(new Uri("fabric:/PingSF/Stateful"), new ServicePartitionKey(0));
                var ms = helloWorldClient.GetCalculationResult(20);

                builder.Append(ms.GetAwaiter().GetResult());

                /*  IMyService helloWorldClient = ServiceProxy.Create<IMyService>( new Uri("fabric:/PingSF/Stateful"), new ServicePartitionKey(1));
                  var ms = helloWorldClient.GetCalculationResult(20);
                  result = ms.Result;*/


            }
            catch (Exception e)
            {
                builder.AppendLine().Append("error from statefull service").Append(e.Message);
            }
            IPingActor1 res = null;

            //FabricClient client = new FabricClient("localhost:19000");

            //builder.Append("good:" + client.FabricSystemApplication + "good");

            //FabricClient fabricClient = this.GetClient(cluster);
            //ServicePartitionList partitions = fabricClient.QueryManager.GetPartitionListAsync(new Uri(serviceUri));

            //long count = 0;
            //foreach (Partition partition in partitions)
            //{
            //    long partitionKey = ((Int64RangePartitionInformation)partition.PartitionInformation).LowKey;
            //    IActorService actorServiceProxy = ActorServiceProxy.Create(new Uri(serviceUri), partitionKey);

            //    ContinuationToken continuationToken = null;

            //    do
            //    {
            //        PagedResult<ActorInformation> page = await actorServiceProxy.GetActorsAsync(continuationToken, CancellationToken.None);

            //        count += page.Items.Where(x => x.IsActive).LongCount();

            //        continuationToken = page.ContinuationToken;
            //    }
            //    while (continuationToken != null);
            //}

            List<Task> ll = new List<Task>();
            for (int i = 0; i < 5; i++)
            {
                int index = i;
                var task = Task.Run(() =>
                 {
                     try
                     {
                         int index_t = index;
                         res = ActorProxy.Create<IPingActor1>(ActorId.CreateRandom());
                         string res1 = res.GetCalculationResult(20).GetAwaiter().GetResult();
                         builder.AppendLine().Append(++index_t).Append(" time paralel:").Append(res1);
                     }
                     catch (Exception e)
                     {
                         builder.AppendLine().Append("error from actor").Append(e.Message);
                     }

                     try
                     {
                         res.SaveState(10);
                     }
                     catch (Exception e)
                     {
                         builder.AppendLine().Append("error from actor save state").Append(e.Message);
                     }
                 });

                ll.Add(task);
            }

            Task.WaitAll(ll.ToArray());

            ViewData["Message"] = builder.ToString();
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
