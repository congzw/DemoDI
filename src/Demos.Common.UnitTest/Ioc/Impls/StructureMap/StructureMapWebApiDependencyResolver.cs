using System;
using System.Collections.Generic;
using StructureMap;

namespace Demos.Common.Ioc.Impls.StructureMap
{
    public class StructureMapDependencyResolver : IMyDependencyResolver
    {
        public IContainer Container { get; set; }

        public StructureMapDependencyResolver(IContainer container)
        {
            Container = container;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public object GetService(Type serviceType)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            throw new NotImplementedException();
        }

        public IMyDependencyScope BeginScope()
        {
            IContainer child = this.Container.GetNestedContainer();
            return new StructureMapDependencyResolver(child);
        }
    }
}
