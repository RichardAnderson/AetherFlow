using AetherFlow.Xrm.Framework.Tests.Example.Interfaces;

namespace AetherFlow.Xrm.Framework.Tests.Example.Implementations
{
    public class InstanceWithOneConstructor : IInstanceWithOneConstructor
    {
        private readonly ICoreInstanceOne _core;
        
        public InstanceWithOneConstructor(ICoreInstanceOne instance)
        {
            _core = instance;
        }

        public bool DoAction() => _core.DoAction();
    }
}
