using AetherFlow.Xrm.Framework.Core;
using AetherFlow.Xrm.Framework.Tests.Content.Actions;
using AetherFlow.Xrm.Framework.Tests.Content.Actions.Config;

namespace AetherFlow.Xrm.Framework.Tests.Content.Plugins
{
    public class UpdateTargetContact : PluginBase
    {
        public UpdateTargetContact(string unSecure, string secure) : base(unSecure, secure) { }
        public UpdateTargetContact() { }

        protected override void Configure(ActionExecutor builder) => 
            builder
                .LoadDependenciesFrom("AetherFlow.Xrm.Framework.Tests.Content.Interfaces")
                .HasSecureConfig<ContactPluginConfig>()
                .OnlyIf(context => context.MessageName == "Create")
                .RunAction<UpdateTargetContactAction>();
    }
}
