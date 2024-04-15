using System;
using AetherFlow.Xrm.Framework.Tests.Interfaces;

namespace AetherFlow.Xrm.Framework.Tests
{
    public abstract class SpecificationBase : ISpecification, IDisposable
    {
        protected Exception ThrownException;
        
        public virtual void RunSpecification()
        {
            Arrange();

            try { Act(); }
            catch (Exception ex) { ThrownException = ex; }
        }

        public virtual void Arrange()
        {
        }

        public virtual void Act()
        {
        }

        public virtual void Cleanup()
        {
        }

        public void Dispose()
        {
            Cleanup();
        }
    }
}
