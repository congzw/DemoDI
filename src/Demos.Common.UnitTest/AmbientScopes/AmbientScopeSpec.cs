using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Demos.Common.AmbientScopes
{
    [TestClass]
    public class AmbientScopeSpecs
    {
        [TestMethod]
        public void GetCurrentScope_InSameScope_Should_Same()
        {
            using (var scope = new AmbientScope())
            {
                var ambientScope = AmbientScope.Current;

                ambientScope.ShouldNotNull();
                ambientScope.ShouldSame(scope);

                var ambientScope2 = AmbientScope.Current;
                ambientScope2.ShouldNotNull();
                ambientScope2.ShouldSame(scope);
                ambientScope2.ShouldSame(ambientScope);
            }
        }

        [TestMethod]
        public void GetCurrentScope_NestedScope_Should_NotSame()
        {
            using (var scopeOuter = new AmbientScope())
            {
                var ambientScopeOuter = AmbientScope.Current;
                ambientScopeOuter.ShouldNotNull();
                ambientScopeOuter.ShouldSame(scopeOuter);

                using (var scope = new AmbientScope())
                {
                    var ambientScope = AmbientScope.Current;

                    ambientScope.ShouldNotNull();
                    ambientScope.ShouldSame(scope);

                    var ambientScope2 = AmbientScope.Current;
                    ambientScope2.ShouldNotNull();
                    ambientScope2.ShouldSame(scope);
                    ambientScope2.ShouldSame(ambientScope);

                    ambientScopeOuter.ShouldNotSame(scope);
                }
            }
        }


        [TestMethod]
        public void Task_GetCurrentScope_SameScope_Should_Same()
        {
            using (var scope = new AmbientScope())
            {
                var ambientScope = AmbientScope.Current;

                ambientScope.ShouldNotNull();
                ambientScope.ShouldSame(scope);

                Task.Run(() =>
                {
                    var ambientScope2 = AmbientScope.Current;
                    ambientScope2.ShouldNotNull();
                    ambientScope2.ShouldSame(scope);
                    ambientScope2.ShouldSame(ambientScope);

                }).Wait();
            }
        }

        [TestMethod]
        public void Task_GetCurrentScope_NestedScope_Should_NotSame()
        {
            using (var scopeOuter = new AmbientScope())
            {
                var ambientScopeOuter = AmbientScope.Current;
                ambientScopeOuter.ShouldNotNull();
                ambientScopeOuter.ShouldSame(scopeOuter);
                using (var scope = new AmbientScope())
                {
                    var ambientScope = AmbientScope.Current;

                    ambientScope.ShouldNotNull();
                    ambientScope.ShouldSame(scope);
                    ambientScopeOuter.ShouldNotSame(scope);

                    Task.Run(() =>
                    {
                        var ambientScope2 = AmbientScope.Current;
                        ambientScope2.ShouldNotNull();
                        ambientScope2.ShouldSame(scope);
                        ambientScope2.ShouldSame(ambientScope);
                        ambientScopeOuter.ShouldNotSame(ambientScope2);

                    }).Wait();
                }
            }
        }


        [TestMethod]
        public void Thread_GetCurrentScope_SameScope_Should_Same()
        {
            using (var scope = new AmbientScope())
            {
                var ambientScope = AmbientScope.Current;

                ambientScope.ShouldNotNull();
                ambientScope.ShouldSame(scope);

                var thread = new Thread(() =>
                {
                    var ambientScope2 = AmbientScope.Current;
                    ambientScope2.ShouldNotNull();
                    ambientScope2.ShouldSame(scope);
                    ambientScope2.ShouldSame(ambientScope);
                });
                thread.Start();
                thread.Join();
            }
        }

        [TestMethod]
        public void Thread_GetCurrentScope_NestedScope_Should_NotSame()
        {
            using (var scopeOuter = new AmbientScope())
            {
                var ambientScopeOuter = AmbientScope.Current;
                ambientScopeOuter.ShouldNotNull();
                ambientScopeOuter.ShouldSame(scopeOuter);
                using (var scope = new AmbientScope())
                {
                    var ambientScope = AmbientScope.Current;

                    ambientScope.ShouldNotNull();
                    ambientScope.ShouldSame(scope);
                    ambientScopeOuter.ShouldNotSame(scope);

                    var thread = new Thread(() =>
                    {
                        var ambientScope2 = AmbientScope.Current;
                        ambientScope2.ShouldNotNull();
                        ambientScope2.ShouldSame(scope);
                        ambientScope2.ShouldSame(ambientScope);
                        ambientScopeOuter.ShouldNotSame(ambientScope2);
                    });
                    thread.Start();
                    thread.Join();
                }
            }
        }
    }
}
