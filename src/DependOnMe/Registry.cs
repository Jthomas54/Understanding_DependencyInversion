using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DependOnMe
{
    /// <summary>
    /// A simple Dependency Injection class that uses constructor injection
    /// </summary>
    public class Registry
    {
        private readonly Dictionary<Type, Type> _internalRegistry;

        /// <summary>
        /// Create an instance of the <see cref="Registry"/> class
        /// </summary>
        public Registry()
        {
            _internalRegistry = new Dictionary<Type, Type>();
        }

        /// <summary>
        /// Register a type and its' concrete implementation
        /// </summary>
        /// <typeparam name="TRequestedType">The type that will be requested to resolve</typeparam>
        /// <typeparam name="TConcreteType">The concrete type that will be created when an instance is requested</typeparam>
        public void Register<TRequestedType, TConcreteType>()
        {
            _internalRegistry.Add(typeof(TRequestedType), typeof(TConcreteType));
        }

        /// <summary>
        /// Removes all registered types
        /// </summary>
        public void Clear()
        {
            _internalRegistry.Clear();
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

        private object GetInstance(Type type)
        {
            if (!IsRegisteredType(type)) throw new Exception("The ctor requires a type that isn't registered with the registry.");

            //Find the type to resolve from the requested type
            var concreteType = _internalRegistry[type];

            return CreateInstance(concreteType);
        }

        /// <summary>
        /// Creates an instance of the provided type as long as it is registered with the registry
        /// </summary>
        /// <param name="t">The type to instantiate</param>
        /// <returns>An instance of the provided type</returns>
        private object CreateInstance(Type t)
        {
            //Get all the public ctors and select the one with the most parameters
            var ctor = t.GetConstructors(BindingFlags.Instance | BindingFlags.Public)
                        .OrderBy(c => c.GetParameters().Length)
                        .LastOrDefault();

            if(ctor == null) throw new Exception(string.Format("{0} doesn't have an accessible ctor", t.Name));

            if (ctor.GetParameters().Any())
            {
                //Check ctor dependencies
                var args = new List<object>();

                foreach (var param in ctor.GetParameters())
                {
                    var paramType = param.ParameterType;

                    args.Add(GetInstance(paramType));

                }

                return Activator.CreateInstance(t, args.ToArray());
            }

            return Activator.CreateInstance(t, null);
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
    }
}
