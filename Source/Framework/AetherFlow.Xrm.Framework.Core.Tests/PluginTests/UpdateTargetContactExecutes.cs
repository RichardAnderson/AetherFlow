using System;
using AetherFlow.Xrm.FakeXrmEasy;
using Microsoft.Xrm.Sdk;
using NUnit.Framework;
using AetherFlow.Xrm.Framework.Tests.Content.Models;
using AetherFlow.Xrm.Framework.Tests.Content.Plugins;

namespace AetherFlow.Xrm.Framework.Tests.PluginTests
{
    [TestFixture]
    public class UpdateTargetContactExecutes : PluginSpecification
    {
        private XrmFakedPluginExecutionContext _context;

        [OneTimeSetUp]
        public void Run() { RunSpecification(); }

        public override void Arrange()
        {
            _context = Context.GetDefaultPluginContext();
            _context.MessageName = "Create";
            _context.InputParameters = new ParameterCollection {
                { "Target", new Entity(Contact.LogicalName, Guid.NewGuid()) }
            };
        }

        public override void Act()
        {
            Context.ExecutePluginWith<UpdateTargetContact>(_context);
        }

        [Test]
        public void EnsureNoExceptionThrown()
        {
            Assert.That(ThrownException, Is.Null);
        }
    }
}
