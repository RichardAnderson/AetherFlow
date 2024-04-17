using System;
using AetherFlow.Xml.Framework.Core.Net.Models;
using AetherFlow.Xml.Framework.Core.Net.Plugins;
using AetherFlow.Xrm.FakeXrmEasy;
using Microsoft.Xrm.Sdk;
using NUnit.Framework;

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
