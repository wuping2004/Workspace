using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Communication.Runtime;

using Microsoft.ServiceFabric.Services;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Microsoft.ServiceFabric.Services.Remoting;
using System.Fabric;
using Microsoft.ServiceFabric.Services.Remoting.V1.FabricTransport.Runtime;
using PingActor1.Interfaces;

namespace Stateful
{

    //class MyCalculation : StatefulService, IMyService
    //{
    //    public MyCalculation(StatefulServiceContext context)
    //    : base(context)
    //    {
    //    }

    //    public Task<string> GetCalculationResult(int input)
    //    {
    //        return Task.FromResult(input + "is calculated..");
    //    }

    //    protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
    //    {
    //        return new[] { new ServiceReplicaListener(context => new FabricTransportServiceRemotingListener(context, this)) };
    //    }

    //}
}
