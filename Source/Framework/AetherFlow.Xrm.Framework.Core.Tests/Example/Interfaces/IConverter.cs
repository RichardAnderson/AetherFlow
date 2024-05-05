using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AetherFlow.Xrm.Framework.Tests.Example.Interfaces
{
    public interface IConverter<T, TO>
    {
        TO From(T input);
        T To(TO input);
    }
}
