using Demos.Common.Ioc.Impls.Mock;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Demos.Common.Ioc
{
    [TestClass]
    public class CoreServiceProviderSpec
    {
        [TestMethod]
        public void Singleton_Should_Same()
        {
            CoreServiceProvider.CurrentFunc = () => new MockDependencyResolver(() => new MockContainer(() => new MockUow()));
            using (var resolver = CoreServiceProvider.Current)
            {
                var singleton = resolver.GetService<MockPerSingleton>();
                singleton.CreatedBy.ShouldEqual("Default");

                var singleton2 = resolver.GetService<MockPerSingleton>();
                singleton2.CreatedBy.ShouldEqual("Default");

                singleton2.ShouldSame(singleton);
            }
        }

        [TestMethod]
        public void Singleton_Nested_Should_Same()
        {
            CoreServiceProvider.CurrentFunc = () => new MockDependencyResolver(() => new MockContainer(() => new MockUow()));
            using (var resolver = CoreServiceProvider.Current)
            {
                var singleton = resolver.GetService<MockPerSingleton>();
                singleton.CreatedBy.ShouldEqual("Default");
                //singleton.LogHashCode();

                using (var childScope = resolver.BeginScope())
                {
                    var child = childScope.GetService<MockPerSingleton>();
                    child.CreatedBy.ShouldEqual("Default.Child");
                    //child.LogHashCode();
                    child.ShouldSame(singleton);
                }
            }
        }

        [TestMethod]
        public void Session_InSameUowScope_Should_Same()
        {
            var mockUow = new MockUow();
            CoreServiceProvider.CurrentFunc = () => new MockDependencyResolver(() => new MockContainer(() => mockUow));
            using (var resolver = CoreServiceProvider.Current)
            {
                var session = resolver.GetService<MockPerSession>();
                session.CreatedBy.ShouldEqual("Default");

                var session2 = resolver.GetService<MockPerSession>();
                session2.CreatedBy.ShouldEqual("Default");

                session2.ShouldSame(session);
            }
        }

        [TestMethod]
        public void Session_InDiffUowScope_Should_Diff()
        {
            var mockUow = new MockUow();
            CoreServiceProvider.CurrentFunc = () => new MockDependencyResolver(() => new MockContainer(() => mockUow));
            using (var resolver = CoreServiceProvider.Current)
            {
                var session = resolver.GetService<MockPerSession>();
                session.CreatedBy.ShouldEqual("Default");

                mockUow = new MockUow();
                var session2 = resolver.GetService<MockPerSession>();
                session2.CreatedBy.ShouldEqual("Default");

                session2.ShouldNotSame(session);
            }
        }

        [TestMethod]
        public void Request_Should_Diff()
        {
            CoreServiceProvider.CurrentFunc = () => new MockDependencyResolver(() => new MockContainer(() => new MockUow()));
            using (var resolver = CoreServiceProvider.Current)
            {
                var request = resolver.GetService<MockPerRequest>();
                request.CreatedBy.ShouldEqual("Default");

                var request2 = resolver.GetService<MockPerRequest>();
                request2.CreatedBy.ShouldEqual("Default");

                request2.ShouldNotSame(request);
            }
        }

        [TestMethod]
        public void Request_Nested_Should_Diff()
        {
            CoreServiceProvider.CurrentFunc = () => new MockDependencyResolver(() => new MockContainer(() => new MockUow()));
            using (var resolver = CoreServiceProvider.Current)
            {
                var request = resolver.GetService<MockPerRequest>();
                request.CreatedBy.ShouldEqual("Default");

                using (var childScope = resolver.BeginScope())
                {
                    var request2 = childScope.GetService<MockPerRequest>();
                    request2.CreatedBy.ShouldEqual("Default.Child");
                    request2.ShouldNotSame(request);
                }
            }
        }
    }
}
