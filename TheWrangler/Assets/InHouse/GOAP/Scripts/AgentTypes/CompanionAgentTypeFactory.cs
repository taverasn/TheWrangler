using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Runtime;

namespace TheWrangler.GOAP
{
    public class CompanionAgentTypeFactory : AgentTypeFactoryBase
    {
        public override IAgentTypeConfig Create()
        {
            var factory = new AgentTypeBuilder("CompanionAgent");

            factory.AddCapability<IdleCapabilityFactory>();
            factory.AddCapability<NeedsCapabilityFactory>();

            return factory.Build();
        }
    }
}