using Demos.Common.AmbientScopes;
using Demos.Common.Ioc.Impls.Mock;
using Demos.Common.Ioc.ScopeContexts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace Demos.Common.Ioc.Impls.StructureMap
{
    [TestClass]
    public class AmbientScopeContextSpec
    {
        [TestMethod]
        public void AmbientScope_Switch_Should_OK()
        {
            using (var resolver = createRootDependencyResolver())
            {
                var ambientScopeContext = createAmbientScopeContext(resolver);

                var myDependencyScope1_1 = ambientScopeContext.Current;
                var myDependencyScope1_2 = ambientScopeContext.Current;
                myDependencyScope1_1.ShouldNotSame(myDependencyScope1_2);

                using (var ambientScope = new AmbientScope())
                {
                    var myDependencyScope2_1 = ambientScopeContext.Current;
                    var myDependencyScope2_2 = ambientScopeContext.Current;
                    myDependencyScope2_1.ShouldSame(myDependencyScope2_2);
                    myDependencyScope2_1.ShouldNotSame(myDependencyScope1_1);
                    
                    using (var ambientScope3 = new AmbientScope())
                    {
                        var myDependencyScope3_1 = ambientScopeContext.Current;
                        var myDependencyScope3_2 = ambientScopeContext.Current;
                        myDependencyScope3_1.ShouldSame(myDependencyScope3_2);
                        myDependencyScope3_1.ShouldNotSame(myDependencyScope1_1);
                        myDependencyScope3_1.ShouldNotSame(myDependencyScope2_1);
                    }
                }
            }
        }
        
        [TestMethod]
        public void Singleton_Should_Same()
        {
            var disposed1 = false;

            using (var resolver = createRootDependencyResolver())
            {
                var ambientScopeContext = createAmbientScopeContext(resolver);
                using (var ambientScope = new AmbientScope())
                {
                    var myDependencyScope = ambientScopeContext.Current;
                    var singleton = myDependencyScope.GetService<MockPerSingleton>();
                    var singleton2 = myDependencyScope.GetService<MockPerSingleton>();

                    singleton.WhenDispose = () => disposed1 = true;
                    singleton2.ShouldSame(singleton);
                    disposed1.ShouldFalse();
                }
                disposed1.ShouldFalse();
            }
            disposed1.ShouldTrue();
        }

        [TestMethod]
        public void Singleton_Nested_Should_Same()
        {
            var disposed1 = false;

            using (var resolver = createRootDependencyResolver())
            {
                var ambientScopeContext = createAmbientScopeContext(resolver);
                using (var ambientScope = new AmbientScope())
                {
                    var singleton = ambientScopeContext.Current.GetService<MockPerSingleton>();
                    var singleton2 = ambientScopeContext.Current.GetService<MockPerSingleton>();

                    singleton.WhenDispose = () => disposed1 = true;
                    singleton2.ShouldSame(singleton);

                    using (var childScope = new AmbientScope())
                    {
                        var child = ambientScopeContext.Current.GetService<MockPerSingleton>();
                        child.ShouldSame(singleton);
                        disposed1.ShouldFalse();
                    }
                    disposed1.ShouldFalse();
                }
            }
            disposed1.ShouldTrue();
        }
        
        [TestMethod]
        public void Session_InSameUowScope_Should_Same()
        {
            var disposed1 = false;
            using (var resolver = createRootDependencyResolver())
            {
                var ambientScopeContext = createAmbientScopeContext(resolver);
                using (var ambientScope = new AmbientScope())
                {
                    var session = ambientScopeContext.Current.GetService<MockPerSession>();
                    var session2 = ambientScopeContext.Current.GetService<MockPerSession>();
                    session.ShouldSame(session2);
                    session.WhenDispose = () => disposed1 = true;
                    disposed1.ShouldFalse();
                }
                disposed1.ShouldTrue();
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

        private IMyDependencyResolver createRootDependencyResolver()
        {
            return new StructureMapDependencyResolver(createContainer());
        }

        private IScopeContext createAmbientScopeContext(IMyDependencyResolver resolver)
        {
            var ambientScopeContext = new AmbientScopeContext(resolver);
            return ambientScopeContext;
        }
    }
}
