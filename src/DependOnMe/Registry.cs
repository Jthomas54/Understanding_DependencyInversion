using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DependOnMe
{
    /// <summary>
    /// A simple Dependency Inversion class
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
            //Find the type to resolve from the requested type
            var concreteType = _internalRegistry[type];

            var ctors = concreteType.GetConstructors();

            if (ctors.Count() > 1)
            {
                //Get the ctor that has the most parameters
                var maxParameters = ctors.Max(c => c.GetParameters().Count());
                var targetCtor = ctors.First(c => c.GetParameters().Count() == maxParameters);

                //Check ctor dependencies
                var args = new List<object>();

                foreach (var param in targetCtor.GetParameters())
                {
                    var paramType = param.ParameterType;

                    if (IsRegisteredType(paramType))
                    {
                        args.Add(GetInstance(paramType));
                    }
                    else
                    {
                        //TODO (Justin): make special exception for this?
                        throw new Exception("The ctor requires a type that isn't registered with the registry.");
                    }
                }

                return Activator.CreateInstance(concreteType, args.ToArray());
            }

            return Activator.CreateInstance(concreteType, null);
        }

        /// <summary>
        /// Checks to see if a type is registered
        /// </summary>
        /// <param name="requestedType">The type to check if it is registered with the registry</param>
        /// <returns>A boolean value determining if the type is registered</returns>
        public bool IsRegisteredType (Type requestedType)
        {
            return _internalRegistry.ContainsKey(requestedType);
        }
    }
}
