using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependOnMe.Tests.Mocks
{
    public interface ICarFactory
    {
        ICar CarBeingProduced { get; }
    }

    //Bad example, but just for testing sub-dependencies
    public class CarFactory:ICarFactory
    {
        public ICar CarBeingProduced { get; private set; }

        public CarFactory(ICar carToProduce)
        {
            CarBeingProduced = carToProduce;
        }
    }
}
