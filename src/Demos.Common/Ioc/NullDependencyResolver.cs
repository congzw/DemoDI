using System;
using System.Collections.Generic;
using System.Linq;

namespace Demos.Common.Ioc
{
    public class NullDependencyResolver : IMyDependencyResolver
    {
        public object GetService(Type serviceType)
        {
            return null;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return Enumerable.Empty<object>();
        }

        public IMyDependencyScope BeginScope()
        {
            return new NullDependencyResolver() { Parent = this };
        }

        public void Dispose()
        {
            Parent = null;
        }

        public IMyDependencyScope Parent { get; set; }
    }
}
