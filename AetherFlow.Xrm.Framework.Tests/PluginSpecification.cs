using AetherFlow.Xrm.FakeXrmEasy;
using AetherFlow.Xrm.Framework.Core.Interfaces;
using NUnit.Framework;

namespace AetherFlow.Xrm.Framework.Tests
{
    public class PluginSpecification : SpecificationBase
    {
        protected IDataverseContainer Container;
        protected XrmFakedContext Context;

        [OneTimeSetUp]
        public void SetupCrmService()
        {
            Context = new XrmFakedContext();
        }
    }
}
