using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DependOnMe
{
    public class ConstructorConfig
    {
        public Type ConcreteType { get; private set; }
        public ConstructorInfo Constructor { get; private set; }

        public ConstructorConfig(Type t, ConstructorInfo ctorInfo)
        {
            ConcreteType = t;
            Constructor = ctorInfo;
        }
    }
}
