using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DependOnMe
{
    public class Registry
    {
        private readonly Dictionary<Type, Type> _internalRegistry;

        public Registry()
        {
            _internalRegistry = new Dictionary<Type, Type>();
        }

        public void Register<TRequestedType, TConcreteType>()
        {
            _internalRegistry.Add(typeof(TRequestedType), typeof(TConcreteType));
        }

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

        public bool IsRegisteredType (Type requestedType)
        {
            return _internalRegistry.ContainsKey(requestedType);
        }
    }
}
