using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AetherFlow.Xrm.Framework.Core.Interfaces;

namespace AetherFlow.Xrm.Framework.Core
{
    public class DataverseContainer : IDataverseContainer
    {
        private readonly IDictionary<Type, Type> _implementations = new Dictionary<Type, Type>();
        private readonly IList<object> _services = new List<object>();

        public void Initialize(Assembly assembly, string rootNamespace)
        {
            // Use reflection to get a list of types
            var types = assembly.GetTypes()
                .Where(t => t.Namespace != null && t.Namespace.StartsWith(rootNamespace) && t.IsInterface)
                .ToArray();

            // Loop through the types and register an implementation
            // We want to avoid abstract classes, and any classes that include the word "Mock" or "Base"
            foreach (var type in types)
            {
                // Get the implementation of the interface
                var implementation = GetImplementationOf(assembly, type);

                // If we have an implementation, add it to the dictionary
                // We can use this to build it later should it be needed!
                if (implementation != null)
                    _implementations.Add(type, implementation);
            }
        }

        /// <summary>
        /// Get an implementation for a given interface from a defined
        /// assembly.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private Type GetImplementationOf(Assembly assembly, Type type) => 
            assembly
                .GetTypes()
                    .FirstOrDefault(t =>
                        t.Namespace != null
                        && t.IsClass
                        && !t.IsAbstract
                        && type.IsAssignableFrom(t)
                        && !t.Name.ToLower().StartsWith("mock")
                        && !t.Name.ToLower().EndsWith("mock")
                        && !t.Name.ToLower().StartsWith("base")
                        && !t.Name.ToLower().EndsWith("base")
                    );

        /// <summary>
        /// Get the best constructor for an implementation, based on the
        /// number of parameters and if they can be satisfied via CI/CD
        /// </summary>
        /// <param name="constructors"></param>
        /// <returns></returns>
        private ConstructorInfo GetBestConstructor(IEnumerable<ConstructorInfo> constructors) => 
            constructors
                .Where(a => a.GetParameters().All(b => b.ParameterType.IsInterface))
                .Where(a => a.GetParameters().All(b => _implementations.ContainsKey(b.ParameterType)))
                .OrderByDescending(a => a.GetParameters().Length)
                .FirstOrDefault();

        public void Add<TKey, T>()
        {
            // This is used to allow us to override the default
            // implementation of a service with a MockService
            _implementations[typeof(TKey)] = typeof(T);
        }

        public void Add<TKey>(object instance)
        {
            // This is to be able to register a new object into the container
            _services.Add(instance);
        }

        public T Get<T>()
        {
            // We want to validate that the type the user is attempting to get
            // is always an interface... we don't want them to be able to specify 
            // a class, as we always want to control the creation of the class
            if (!typeof(T).IsInterface)
                throw new InvalidOperationException("You can only get an instance of an interface");

            // Get the service from the services list if
            // and return it, but only if it exists
            var service = _services.OfType<T>().FirstOrDefault();
            if (service != null) return service;

            // We did not find a service, therefore, we should create
            // a new instance of the service through dependency injection
            // and then return it!
            service = (T)Get(typeof(T));
            return service;
        }

        private object Get(Type type)
        {
            // We need to once again attempt to get the implementation, as this
            // function is recursive
            var service = _services.FirstOrDefault(type.IsInstanceOfType);
            if (service != null) return service;

            // We now need to get the implementation of the interface
            // We should check we actually have one first
            if (!_implementations.ContainsKey(type))
                throw new InvalidOperationException($"No implementation found for {type.Name}");

            // Get the implementation and create an instance of it
            var implementation = _implementations[type];
            var constructor = GetBestConstructor(implementation.GetConstructors());
            var parameters = constructor?.GetParameters().Select(a => Get(a.ParameterType)).ToArray();
            var instance = Activator.CreateInstance(implementation, parameters);

            // Register the instance as a singleton in the 
            // services list
            _services.Add(instance);

            // return the instance
            return instance;
        }
    }
}
