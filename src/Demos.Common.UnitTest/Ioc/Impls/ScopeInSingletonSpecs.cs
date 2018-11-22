using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Demos.Common.Ioc.Impls
{
    [TestClass]
    public class ScopeInSingletonSpecs
    {
        [TestMethod]
        public void SameThread_Should_Same()
        {
            using (var kernel = new StandardKernel())
            {
                kernel.Bind<object>().ToSelf().InSingletonScope();
                var instance1 = kernel.Get<object>();
                var instance2 = kernel.Get<object>();
                instance1.ShouldSame(instance2);
            }
        }
        
        [TestMethod]
        public void ThreadStart_Should_Same()
        {
            using (var kernel = new StandardKernel())
            {
                kernel.Bind<object>().ToSelf().InSingletonScope();

                var instance1 = kernel.Get<object>();
                object instance2 = null;
                var thread = new Thread(() =>
                {
                    instance2 = kernel.Get<object>();
                });
                thread.Start();
                thread.Join();

                instance1.ShouldSame(instance2);
            }
        }

        [TestMethod]
        public void TaskRun_Should_Same()
        {
            using (var kernel = new StandardKernel())
            {
                kernel.Bind<object>().ToSelf().InSingletonScope();

                for (int i = 0; i < 100; i++)
                {
                    var instance1 = kernel.Get<object>();
                    object instance2 = null;
                    Task.Run(() =>
                    {
                        instance2 = kernel.Get<object>();
                    }).Wait();

                    instance1.ShouldSame(instance2);
                }
            }
        }
    }
}
