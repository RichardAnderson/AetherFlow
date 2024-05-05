using AetherFlow.Xrm.Framework.Core.Interfaces;
using AetherFlow.Xrm.Framework.Core;
using AetherFlow.Xrm.Framework.Tests.Example.Interfaces;
using NUnit.Framework;

namespace AetherFlow.Xrm.Framework.Tests.DataverseContainerTests
{
    public class CheckBestConstructorUsed : SpecificationBase
    {
        [OneTimeSetUp]
        public void Run() { RunSpecification(); }

        // ARRANGE variables
        private IDataverseContainer _container;

        // ACT variables
        private ICoreInstanceThree _instance;

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
            _instance = _container.Get<ICoreInstanceThree>();
        }

        [Test]
        public void EnsureNoExceptionThrown()
        {
            Assert.That(ThrownException, Is.Null);
        }

        [Test]
        public void EnsureActionReturnsTrue()
        {
            Assert.That(_instance.DoAction(), Is.True);
        }
    }
}