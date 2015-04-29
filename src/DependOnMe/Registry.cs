using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


//TODO: Support generic types
//TODO: Support lifetime management
//TODO: Use Expressions instead?
namespace DependOnMe
{
    /// <summary>
    /// A simple Dependency Injection class that uses constructor injection
    /// </summary>
    public class Registry
    {
        private readonly Dictionary<Type, ConstructorConfig> _internalRegistry;

        /// <summary>
        /// Create an instance of the <see cref="Registry"/> class
        /// </summary>
        public Registry()
        {
            _internalRegistry = new Dictionary<Type, ConstructorConfig>();
        }

        /// <summary>
        /// Register a type and its' concrete implementation
        /// </summary>
        /// <typeparam name="TRequestedType">The type that will be requested to resolve</typeparam>
        /// <typeparam name="TConcreteType">The concrete type that will be created when an instance is requested</typeparam>
        public void Register<TRequestedType, TConcreteType>()
        {
            var t = typeof (TConcreteType);
            var config = new ConstructorConfig(t, GetConstructor(t));

            _internalRegistry.Add(typeof(TRequestedType), config);
        }

        /// <summary>
        /// Gets an instance of the requested type
        /// </summary>
        /// <typeparam name="TRequestedType">The type to resolve</typeparam>
        /// <returns>A resolved instance of the requested type</returns>
        public TRequestedType GetInstance<TRequestedType>()
        {
            return (TRequestedType) GetInstance(typeof (TRequestedType));
        }

        private object GetInstance(Type t)
        {
            if (!IsRegisteredType(t)) throw new Exception("The ctor requires a type that isn't registered with the registry.");

            return CreateInstance(t);
        }

        /// <summary>
        /// Get the greediest constructor
        /// </summary>
        /// <param name="t">The type to get the greediest constructor's <see cref="ConstructorInfo"/></param>
        /// <returns><see cref="ConstructorInfo"/> of the greediest ctor</returns>
        private ConstructorInfo GetConstructor(Type t)
        {
            var ctorInfo = t.GetConstructors(BindingFlags.Instance | BindingFlags.Public)
                        .OrderBy(c => c.GetParameters().Length)
                        .LastOrDefault();

            if (ctorInfo == null) throw new Exception(string.Format("{0} doesn't have an accessible ctor", t.Name));

            return ctorInfo;
        }

        /// <summary>
        /// Creates an instance of the provided type as long as it is registered with the registry
        /// </summary>
        /// <param name="t">The type to instantiate</param>
        /// <returns>An instance of the provided type</returns>
        private object CreateInstance(Type t)
        {
            var config = _internalRegistry[t];

            if (config.HasParameters)
            {
                //Check ctor dependencies
                var args = config.Constructor.GetParameters().Select(param => GetInstance(param.ParameterType));

                return Activator.CreateInstance(config.ConcreteType, args.ToArray());
            }
            
            return Activator.CreateInstance(config.ConcreteType, null);
        }

        /// <summary>
        /// Checks to see if a type is registered
        /// </summary>
        /// <param name="requestedType">The type to check if it is registered with the registry</param>
        /// <returns>A boolean value determining if the type is registered</returns>
        public bool IsRegisteredType(Type requestedType)
        {
            return _internalRegistry.ContainsKey(requestedType);
        }

        /// <summary>
        /// Removes all registered types
        /// </summary>
        public void Clear()
        {
            _internalRegistry.Clear();
        }
    }
}
