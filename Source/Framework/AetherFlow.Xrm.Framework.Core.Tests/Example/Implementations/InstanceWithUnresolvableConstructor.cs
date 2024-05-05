using System;
using AetherFlow.Xrm.Framework.Tests.Example.Interfaces;

namespace AetherFlow.Xrm.Framework.Tests.Example.Implementations
{
    public class InstanceWithUnresolvableConstructor : IInstanceWithUnresolvableConstructor
    {
        public InstanceWithUnresolvableConstructor(IServiceProvider provider)
        {

        }

        public bool DoAction() => true;
    }
}
