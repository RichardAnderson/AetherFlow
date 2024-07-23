using System;
using AetherFlow.Xrm.FakeXrmEasy;
using AetherFlow.Xrm.Framework.Core.Helpers;
using Microsoft.Xrm.Sdk;
using NUnit.Framework;
using AetherFlow.Xrm.Framework.Tests.Content.Models;
using AetherFlow.Xrm.Framework.Tests.Content.Plugins;
using AetherFlow.Xrm.Framework.Tests.Content.Actions.Config;

namespace AetherFlow.Xrm.Framework.Tests.PluginTests
{
    [TestFixture]
    public class UpdateTargetContactExecutes : PluginSpecification
    {
        private XrmFakedPluginExecutionContext _context;
        private Entity _target;
        private ContactPluginConfig _config;

        [OneTimeSetUp]
        public void Run() { RunSpecification(); }

        public override void Arrange()
        {
            _target = new Entity(Contact.LogicalName, Guid.NewGuid());
            _config = new ContactPluginConfig { FirstName = "Test Name" };

            _context = Context.GetDefaultPluginContext();
            _context.MessageName = "Create";
            _context.InputParameters = new ParameterCollection {
                { "Target", _target }
            };
        }
        
        public override void Act()
        {
            Context.ExecutePluginWithConfigurations<UpdateTargetContact>(
                _context, 
                "",
                new JsonContractSerializer().Serialize(_config)
            );
        }

        [Test]
        public void EnsureNoExceptionThrown()
        {
            Assert.That(ThrownException, Is.Null);
        }

        [Test]
        public void EnsureContactIsUpdated()
        {
            Assert.That(_target.GetAttributeValue<string>("firstname"), Is.EqualTo(_config.FirstName));
        }
    }
}
