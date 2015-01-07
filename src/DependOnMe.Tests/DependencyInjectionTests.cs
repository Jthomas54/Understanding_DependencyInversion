using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using DependOnMe.Tests.Mocks;

namespace DependOnMe.Tests
{
    [TestClass]
    public class DependencyInjectionTests
    {
        private Registry _registry;

        [TestInitialize]
        public void Init()
        {
            _registry = new Registry();
        }

        [TestMethod]
        public void VerifyCorrectTypeCreated_WhenRequestedTypeIsResolved()
        {
            _registry.Register<ICar, Chevy>();

            var resolvedType = _registry.GetInstance<ICar>();

            Assert.IsInstanceOfType(resolvedType, typeof(Chevy));

            Debug.WriteLine(resolvedType.ToString());
        }

        [TestMethod]
        public void CreateType_WhenNoUserDefinedCtor_IsPresent()
        {
            _registry.Register<ICar, Ford>();

            var resolvedType = _registry.GetInstance<ICar>();

            Assert.IsNotNull(resolvedType);

            Debug.WriteLine(resolvedType.ToString());
        }

        [TestMethod]
        public void VerifySubDependenciesAreCreated_WhenTopLevelClassResolved()
        {
            _registry.Register<ICarFactory, CarFactory>();
            _registry.Register<ICar, Chevy>();

            var resolvedFactory = _registry.GetInstance<ICarFactory>();

            Assert.IsNotNull(resolvedFactory);
            Assert.IsInstanceOfType(resolvedFactory.CarBeingProduced, typeof(Chevy));

            Debug.WriteLine(resolvedFactory.CarBeingProduced.ToString());
        }
    }
}
