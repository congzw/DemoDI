using System;

namespace Demos.Common.Ioc
{
    public interface IMyDependencyResolver : IMyDependencyScope, IDisposable
    {
        IMyDependencyScope BeginScope();
    }
}