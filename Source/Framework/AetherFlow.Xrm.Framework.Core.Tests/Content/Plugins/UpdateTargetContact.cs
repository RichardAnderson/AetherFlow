using AetherFlow.Xrm.Framework.Core;
using AetherFlow.Xrm.Framework.Core.Interfaces;
using AetherFlow.Xrm.Framework.Tests.Content.Interfaces;
using Microsoft.Xrm.Sdk;
using AetherFlow.Xrm.Framework.Tests.Content.Models;

namespace AetherFlow.Xrm.Framework.Tests.Content.Plugins
{
    public class UpdateTargetContact : PluginBase
    {
        public UpdateTargetContact(string unsecure, string secure) { }
        public UpdateTargetContact() { }

        protected override void ExecuteCrmPlugin(IDataverseContainer container)
        {
            // Initialize our interfaces in the container
            container.Initialize(GetType().Assembly, "AetherFlow.Xrm.Framework.Tests.Content.Interfaces");

            // Get the Contact DAL
            var contactDal = container.Get<IContactDal>();

            // Get target
            var context = container.Get<IPluginExecutionContext>();
            var target = (Entity)context.InputParameters["Target"];

            // Get the contact object, update values & save
            // Save actually updates the target object in this instance
            var contact = contactDal.FromTarget(target);
            contact.StateCode = Contact.Choices.StateCode.Inactive;
            contact.FirstName = "Test";
            contact.Save();
        }
    }
}
