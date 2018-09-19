using Common;
using Microsoft.ServiceFabric.Services.Remoting;
using Microsoft.ServiceFabric.Services.Remoting.FabricTransport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

[assembly: FabricTransportServiceRemotingProvider(MaxMessageSize = 134217728, RemotingListenerVersion = Microsoft.ServiceFabric.Services.Remoting.RemotingListenerVersion.V2_1, RemotingClientVersion = Microsoft.ServiceFabric.Services.Remoting.RemotingClientVersion.V2_1)]
namespace Common
{
    public interface IMyService : IService
    {
        Task<List<Fixture>> GetFixtureList();
    }

    public interface IMyService2 : IService
    {
        Task<string> GetFixtureList();
    }
}
