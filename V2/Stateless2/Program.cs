using System;
using System.Diagnostics;
using System.Fabric;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Client;
using Microsoft.ServiceFabric.Services.Runtime;

namespace Stateless2
{
    internal static class Program
    {
        /// <summary>
        /// This is the entry point of the service host process.
        /// </summary>
        private static void Main()
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                IMyService2 helloWorldClient = null;
                string res = null;
                try
                {

                    var proxyFactory = new ServiceProxyFactory((c) =>
                    {
                        return new FabricTransportServiceRemotingClientFactory();
                    });

                    helloWorldClient = proxyFactory.CreateServiceProxy<IMyService2>(new Uri("fabric:/PingSF/Stateless"));
                    res = helloWorldClient.GetFixtureList().GetAwaiter().GetResult();

                }
                catch (Exception e)
                {
                    builder.AppendLine().Append("error from statefull service").Append(e.Message);
                }

                
            }
            catch (Exception e)
            {
                ServiceEventSource.Current.ServiceHostInitializationFailed(e.ToString());
                throw;
            }
        }
    }
}
