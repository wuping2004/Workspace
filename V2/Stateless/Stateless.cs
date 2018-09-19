using System;
using System.Collections.Generic;
using System.Fabric;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Microsoft.ServiceFabric.Services.Remoting.FabricTransport;
using Microsoft.ServiceFabric.Services.Remoting.FabricTransport.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Client;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;

[assembly: FabricTransportServiceRemotingProvider(MaxMessageSize = 134217728)]
namespace Stateless
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class Stateless : StatelessService, IMyService2
    {
        public Stateless(StatelessServiceContext context)
            : base(context)
        { }

        public Task<string> GetFixtureList()
        {

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
                List<Fixture> fixtureList = helloWorldClient.GetFixtureList().GetAwaiter().GetResult();
                int index = 1;
                builder.AppendLine("it works for V2!");
                foreach (var item in fixtureList)
                {
                    builder.AppendLine(string.Format("fixture object:{0},", index++))
                            .AppendLine(string.Format("{0}:{1},", "FixtureId", item.FixtureId))
                            .AppendLine(string.Format("{0}:{1},", "HomeTeam", item.HomeTeam))
                            .AppendLine(string.Format("{0}:{1},", "IsSettled", item.IsSettled))
                            .AppendLine(string.Format("{0}:{1},", "StartTime", item.StartTime.ToString()))
                            .AppendLine(string.Format("{0}:{1},", "Venue", item.Venue.ToString()))
                            .AppendLine(string.Format("{0}:{1}", "AwayTeam count", item.AwayTeam.Count));
                }
            }
            catch (Exception e)
            {
                builder.AppendLine().Append("error from statefull service").Append(e.Message);
            }

            return Task.FromResult(builder.ToString());
        }

        /// <summary>
        /// Optional override to create listeners (e.g., TCP, HTTP) for this service replica to handle client or user requests.
        /// </summary>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new[]
            {
                 new ServiceInstanceListener((c) =>
                 {
                     var settings = new FabricTransportRemotingListenerSettings();
                     settings.UseWrappedMessage = true;
                     return new FabricTransportServiceRemotingListener(c, this,settings);

                 })
             };

        }

        /// <summary>
        /// This is the main entry point for your service instance.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service instance.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following sample code with your own logic 
            //       or remove this RunAsync override if it's not needed in your service.

            long iterations = 0;

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                ServiceEventSource.Current.ServiceMessage(this.Context, "Working-{0}", ++iterations);

                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            }
        }
    }
}
