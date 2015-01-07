using System;

namespace DependOnMe.Tests.Mocks
{
    public interface ICar
    {
        string Make { get; }
        string Model { get; }
    }

    public class Chevy : ICar
    {
        public string Make { get; private set; }
        public string Model { get; private set; }

        public Chevy()
        {
            Make = "Chevy";
            Model = "Malibu";
        }

        public override string ToString()
        {
            return string.Format("I'm a {0} {1}!", Make, Model);
        }
    }

    //Used to test for no user defined ctor
    public class Ford : ICar
    {
        public string Make { get; private set; }
        public string Model { get; private set; }

        public override string ToString()
        {
            return "I'm supposed to be a Ford...";
        }
    }
}
