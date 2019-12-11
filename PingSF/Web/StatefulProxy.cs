using Microsoft.ServiceFabric.Services.Remoting;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using PingActor1.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web
{
    public class StatefulProxy
    {

        static string GetCalculationResult()
        {
            IMyService helloWorldClient = ServiceProxy.Create<IMyService>(new Uri("fabric:/MyApplication/MyHelloWorldService"));

            var message = helloWorldClient.GetCalculationResult(20);
            return message.Result;
        }
    }
}
