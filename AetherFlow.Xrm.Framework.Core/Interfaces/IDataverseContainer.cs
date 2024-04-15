using System.Reflection;

namespace AetherFlow.Xrm.Framework.Core.Interfaces
{
    public interface IDataverseContainer
    {
        void Initialize(Assembly assembly, string rootNamespace);
        void Add<TKey, T>();
        void Add<TKey>(object instance);
        T Get<T>();
    }
}