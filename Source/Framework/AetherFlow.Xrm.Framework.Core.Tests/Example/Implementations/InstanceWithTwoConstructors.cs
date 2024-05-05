using AetherFlow.Xrm.Framework.Tests.Example.Interfaces;

namespace AetherFlow.Xrm.Framework.Tests.Example.Implementations
{
    public class InstanceWithTwoConstructors : IInstanceWithTwoConstructors
    {
        private readonly ICoreInstanceOne _core;
        
        public InstanceWithTwoConstructors(ICoreInstanceOne coreInstanceOne)
        {
            _core = coreInstanceOne;
        }

        public InstanceWithTwoConstructors()
        {
        }

        public bool DoAction() => _core.DoAction();
    }
}
