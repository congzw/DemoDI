using System;
using System.Collections.Generic;

namespace Demos.Common.Ioc
{
    public interface IMyDependencyScope : IDisposable
    {
        object GetService(Type serviceType);
        IEnumerable<object> GetServices(Type serviceType);
    }
}
