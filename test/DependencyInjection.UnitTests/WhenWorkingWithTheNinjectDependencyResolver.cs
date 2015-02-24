using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Psns.Common.Test.BehaviorDrivenDevelopment;
using Psns.Common.Mvc.DependencyInjection;

using Ninject;
using Ninject.Activation;
using Moq;

namespace DependencyInjection.UnitTests
{
    public class TestType { }

    public class WhenWorkingWithTheNinjectDependencyResolver : BehaviorDrivenDevelopmentCaseTemplate
    {
        protected NinjectDependencyResolver Resolver;
        protected Mock<IKernel> MockKernel;

        public override void Arrange()
        {
            base.Arrange();

            MockKernel = new Mock<IKernel>();
            Resolver = new NinjectDependencyResolver(MockKernel.Object);
        }
    }

    [TestClass]
    public class AndGettingAService : WhenWorkingWithTheNinjectDependencyResolver
    {
        object _returnedService;

        public override void Arrange()
        {
            base.Arrange();

            MockKernel.Setup(k => k.Resolve(It.IsAny<IRequest>())).Returns(new object[] { new TestType() });
        }

        public override void Act()
        {
            base.Act();

            _returnedService = Resolver.GetService(typeof(TestType));
        }

        [TestMethod]
        public void ThenTheKernelShouldBeCalledToRetrieveTheService()
        {
            Assert.IsInstanceOfType(_returnedService, typeof(TestType));
        }
    }

    [TestClass]
    public class AndGettingSeveralServices : WhenWorkingWithTheNinjectDependencyResolver
    {
        IEnumerable<object> _returnedServices;

        public override void Arrange()
        {
            base.Arrange();

            MockKernel.Setup(k => k.Resolve(It.IsAny<IRequest>())).Returns(new object[] { new TestType(), new string(new [] { 'a' }) });
        }

        public override void Act()
        {
            base.Act();

            _returnedServices = Resolver.GetServices(typeof(TestType));
        }

        [TestMethod]
        public void ThenTheKernelShouldBeCalledToRetrieveTheService()
        {
            Assert.IsInstanceOfType(_returnedServices.ElementAt(0), typeof(TestType));
            Assert.IsInstanceOfType(_returnedServices.ElementAt(1), typeof(string));
        }
    }
}
