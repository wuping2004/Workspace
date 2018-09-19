using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Client;
using Microsoft.ServiceFabric.Data;
using Common;
using Microsoft.ServiceFabric.Services.Remoting;
using Microsoft.ServiceFabric.Services.Remoting.FabricTransport;

[assembly: FabricTransportServiceRemotingProvider(MaxMessageSize = 134217728)]
namespace Stateful
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal sealed class Stateful : StatefulService, IMyService
    {
        public Stateful(StatefulServiceContext context)
            : base(context)
        { }

        public Task<string> GetCalculationResult(int input)
        {
            return Task.Run<string>(() => { return "Stateful service is called!"; });
        }

        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            //listenOnSecondary: true 
            // It will listen to secondary replica, notice that only read is allowed, if listenOnSecondary: true is set
            // return new[] { new ServiceReplicaListener(context => new FabricTransportServiceRemotingListener(context, this), listenOnSecondary: true) };

            return new[] { new ServiceReplicaListener(context => new FabricTransportServiceRemotingListener(context, this)) };
        }

        /// <summary>
        /// This is the main entry point for your service replica.
        /// This method executes when this replica of your service becomes primary and has write status.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service replica.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following sample code with your own logic 
            //       or remove this RunAsync override if it's not needed in your service.

            var myDictionary = await this.StateManager.GetOrAddAsync<IReliableDictionary<string, long>>("myDictionary");

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                using (var tx = this.StateManager.CreateTransaction())
                {
                    var result = await myDictionary.TryGetValueAsync(tx, "Counter");

                    ServiceEventSource.Current.ServiceMessage(this.Context, "Current Counter Value: {0}",
                        result.HasValue ? result.Value.ToString() : "Value does not exist.");

                    await myDictionary.AddOrUpdateAsync(tx, "Counter", 0, (key, value) => ++value);

                    // If an exception is thrown before calling CommitAsync, the transaction aborts, all changes are 
                    // discarded, and nothing is saved to the secondary replicas.
                    await tx.CommitAsync();
                }

                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            }
        }


        Task<List<Fixture>> IMyService.GetFixtureList()
        {

            List<Team> teams = new List<Team>();
            teams.Add(new Team { TeamName = "team_one" });
            teams.Add(new Team { TeamName = "team_two" });

            List<Fixture> fixtures = new List<Fixture>();
            Fixture fix = new Fixture()
            {
                AwayTeam = teams,
                FixtureId = 0,
                HomeTeam = new Team() { TeamName = "hometeam" },
                IsSettled = true,
                StartTime = DateTime.Now,
                Venue = "frist"
            };

            Fixture second = new Fixture()
            {
                AwayTeam = teams,
                FixtureId = 1,
                HomeTeam = new Team() { TeamName = "hometeam1" },
                IsSettled = false,
                StartTime = DateTime.Now,
                Venue = "frist"
            };

            fixtures.Add(fix);
            fixtures.Add(second);

            return Task.FromResult(fixtures);
        }
    }
}
