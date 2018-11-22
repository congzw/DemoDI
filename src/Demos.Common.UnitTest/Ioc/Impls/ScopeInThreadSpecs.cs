using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Demos.Common.Ioc.Impls
{
    [TestClass]
    public class ScopeInThreadSpecs
    {
        [TestMethod]
        public void SameThread_Should_Same()
        {
            using (var kernel = new StandardKernel())
            {
                kernel.Bind<object>().ToSelf().InThreadScope();
                var instance1 = kernel.Get<object>();
                var instance2 = kernel.Get<object>();
                instance1.ShouldSame(instance2);
            }
        }

        [TestMethod]
        public void ThreadStart_Should_NotSame()
        {
            var kernel = new StandardKernel();
            kernel.Bind<object>().ToSelf().InThreadScope();
            
            var instance1 = kernel.Get<object>();
            object instance2 = null;
            var thread = new Thread(() =>
            {
                instance2 = kernel.Get<object>();
            });
            thread.Start();
            thread.Join();

            instance1.ShouldNotSame(instance2);
            kernel.Dispose();
        }
        
        [TestMethod]
        public async Task TaskRun_Should_NotSure()
        {
            for (int i = 0; i < 20; i++)
            {
                var kernel = new StandardKernel();
                kernel.Bind<object>().ToSelf().InThreadScope();
                var instance1 = kernel.Get<object>();
                object instance2 = null;
                await Task.Run(() =>
                {
                    instance2 = kernel.Get<object>();

                });
                
                instance1.LogHashCodeWiths(instance2);
                kernel.Dispose();
            }
        }
    }
}
