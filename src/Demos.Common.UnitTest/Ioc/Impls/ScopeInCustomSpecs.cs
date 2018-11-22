using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Demos.Common.Ioc.Impls
{
    [TestClass]
    public class ScopeInCustomSpecs
    {
        [TestMethod]
        public void SameThread_SameScope_Should_Same()
        {
            using (var kernel = new StandardKernel())
            {
                kernel.Bind<object>().ToSelf().InScope(ctx => User.Current);
                User.Current = new User();
                var instance1 = kernel.Get<object>();
                var instance2 = kernel.Get<object>();
                instance1.ShouldSame(instance2);
            }
        }
        [TestMethod]
        public void SameThread_DiffScope_Should_NotSame()
        {
            using (var kernel = new StandardKernel())
            {
                kernel.Bind<object>().ToSelf().InScope(ctx => User.Current);
                User.Current = new User();
                var instance1 = kernel.Get<object>();
                User.Current = new User();
                var instance2 = kernel.Get<object>();
                instance1.ShouldNotSame(instance2);
            }
        }
        
        [TestMethod]
        public void ThreadStart_SameScope_Should_Same()
        {
            using (var kernel = new StandardKernel())
            {
                kernel.Bind<object>().ToSelf().InScope(ctx => User.Current);
                User.Current = new User();

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
        public void ThreadStart_DiffScope_Should_NotSame()
        {
            using (var kernel = new StandardKernel())
            {
                kernel.Bind<object>().ToSelf().InScope(ctx => User.Current);
                User.Current = new User();

                var instance1 = kernel.Get<object>();
                object instance2 = null;
                var thread = new Thread(() =>
                {
                    User.Current = new User();
                    instance2 = kernel.Get<object>();
                });
                thread.Start();
                thread.Join();

                instance1.ShouldNotSame(instance2);
            }
        }

        [TestMethod]
        public void TaskRun_SameScope_Should_Same()
        {
            using (var kernel = new StandardKernel())
            {
                kernel.Bind<object>().ToSelf().InScope(ctx => User.Current);
                User.Current = new User();

                for (int i = 0; i < 20; i++)
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
        [TestMethod]
        public void TaskRun_DiffScope_Should_NotSame()
        {
            using (var kernel = new StandardKernel())
            {
                kernel.Bind<object>().ToSelf().InScope(ctx => User.Current);

                for (int i = 0; i < 20; i++)
                {
                    User.Current = new User();
                    var instance1 = kernel.Get<object>();
                    object instance2 = null;
                    Task.Run(() =>
                    {
                        User.Current = new User();
                        instance2 = kernel.Get<object>();
                    }).Wait();

                    instance1.ShouldNotSame(instance2);
                }
            }
        }

        #region mocks

        class User
        {
            public string Name { get; set; }
            public static User Current { get; set; }
        }


        #endregion
    }
}
