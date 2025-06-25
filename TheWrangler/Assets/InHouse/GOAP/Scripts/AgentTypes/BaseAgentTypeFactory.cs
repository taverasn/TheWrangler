using CrashKonijn.Docs.GettingStarted.Capabilities;
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Runtime;
using TheWrangler.GOAP.Factories;
using UnityEngine;

namespace TheWrangler.GOAP.AgentTypes
{
    public class BaseAgentTypeFactory : AgentTypeFactoryBase
    {
        [SerializeField] private CapabilityFactory capabilityFactory;
        public override IAgentTypeConfig Create()
        {
            var factory = new AgentTypeBuilder("BaseAgent");
            factory.AddCapability(capabilityFactory);

            return factory.Build();
        }
    }
}