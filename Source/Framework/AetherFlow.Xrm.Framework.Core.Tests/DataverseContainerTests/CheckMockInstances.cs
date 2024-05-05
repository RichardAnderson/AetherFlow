using AetherFlow.Xrm.Framework.Core.Interfaces;
using AetherFlow.Xrm.Framework.Core;
using AetherFlow.Xrm.Framework.Tests.Example.Interfaces;
using NUnit.Framework;

namespace AetherFlow.Xrm.Framework.Tests.DataverseContainerTests
{
    public class CheckMockInstances : SpecificationBase
    {
        [OneTimeSetUp]
        public void Run() { RunSpecification(); }

        // ARRANGE variables
        private IDataverseContainer _container;

        // ACT variables
        private IExampleIntegration _example;

        public override void Arrange()
        {
            _container = new DataverseContainer();
            _container.Initialize(
                GetType().Assembly,
                "AetherFlow.Xrm.Framework.Tests.Example.Interfaces"
            );
        }

        public override void Act()
        {
            _example = _container.Get<IExampleIntegration>();
        }

        [Test]
        public void EnsureExceptionThrown()
        {
            Assert.That(ThrownException, Is.Not.Null);
        }
    }
}