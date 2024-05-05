using AetherFlow.Xrm.Framework.Tests.Example.Interfaces;

namespace AetherFlow.Xrm.Framework.Tests.Example.Implementations
{
    public class ExampleIntegrationMock : IExampleIntegration
    {
        public bool DoAction() => true;
    }
}
