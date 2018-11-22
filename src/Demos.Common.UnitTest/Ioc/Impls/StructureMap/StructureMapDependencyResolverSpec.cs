using Demos.Common.Ioc.Impls.Mock;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace Demos.Common.Ioc.Impls.StructureMap
{
    [TestClass]
    public class StructureMapDependencyResolverSpec
    {
        [TestMethod]
        public void Singleton_Should_Same()
        {
            var container = createContainer();
            using (var myDependencyScope = new StructureMapDependencyResolver(container))
            {
                var singleton = myDependencyScope.GetService<MockPerSingleton>();
                var singleton2 = myDependencyScope.GetService<MockPerSingleton>();
                singleton2.ShouldSame(singleton);
            }
        }

        [TestMethod]
        public void Singleton_Nested_Should_Same()
        {
            var container = createContainer();
            using (var myDependencyScope = new StructureMapDependencyResolver(container))
            {
                var singleton = myDependencyScope.GetService<MockPerSingleton>();

                using (var childScope = myDependencyScope.BeginScope())
                {
                    var child = childScope.GetService<MockPerSingleton>();
                    child.ShouldSame(singleton);
                }
            }
        }

        [TestMethod]
        public void Session_InSameUowScope_Should_Same()
        {
            var container = createContainer();
            using (var myDependencyScope = new StructureMapDependencyResolver(container))
            {
                using (var nestedScope = myDependencyScope.BeginScope())
                {
                    var session = nestedScope.GetService<MockPerSession>();
                    var session2 = nestedScope.GetService<MockPerSession>();
                    session2.ShouldSame(session);
                }
            }
        }

        [TestMethod]
        public void Session_InDiffUowScope_Should_Diff()
        {
            var container = createContainer();
            using (var myDependencyScope = new StructureMapDependencyResolver(container))
            {
                var sessionRoot = myDependencyScope.GetService<MockPerSession>();
                var sessionRoot2 = myDependencyScope.GetService<MockPerSession>();
                sessionRoot.ShouldNotSame(sessionRoot2);

                int session1HashCode;
                using (var nestedScope = myDependencyScope.BeginScope())
                {
                    var session = nestedScope.GetService<MockPerSession>();
                    session1HashCode = session.GetHashCode();
                    session.ShouldNotSame(sessionRoot);
                }

                int session2HashCode;
                using (var nestedScope = myDependencyScope.BeginScope())
                {
                    var session = nestedScope.GetService<MockPerSession>();
                    session2HashCode = session.GetHashCode();
                    session.ShouldNotSame(sessionRoot);
                }

                session1HashCode.ShouldNotSame(session2HashCode);
            }
        }

        [TestMethod]
        public void Request_Should_Diff()
        {
            var container = createContainer();
            using (var myDependencyScope = new StructureMapDependencyResolver(container))
            {
                var request = myDependencyScope.GetService<MockPerRequest>();
                var request2 = myDependencyScope.GetService<MockPerRequest>();
                request2.ShouldNotSame(request);
            }
        }

        [TestMethod]
        public void Request_Nested_Should_Diff()
        {
            var container = createContainer();
            using (var myDependencyScope = new StructureMapDependencyResolver(container))
            {
                var request = myDependencyScope.GetService<MockPerRequest>();

                using (var nestedScope = myDependencyScope.BeginScope())
                {
                    var request2 = nestedScope.GetService<MockPerRequest>();
                    var request3 = nestedScope.GetService<MockPerRequest>();
                    request2.ShouldNotSame(request3);
                    request2.ShouldNotSame(request);
                }
            }
        }

        private IContainer createContainer()
        {
            var container = new Container();

            container.Configure(cfg =>
            {
                cfg.For<MockPerSingleton>().Singleton();
                cfg.For<MockPerSession>();
                cfg.For<MockPerRequest>().AlwaysUnique();
            });

            return container;
        }
    }
}
