using System;
using System.Collections.Generic;
using System.Linq;
using StructureMap;

namespace Demos.Common.Ioc.Impls.StructureMap
{
    public class StructureMapDependencyResolver : IMyDependencyResolver
    {
        protected IContainer Container { get; set; }

        public StructureMapDependencyResolver(IContainer container)
        {
            Container = container;
        }

        public void Dispose()
        {
            if (Container == null)
            {
                return;
            }
            Container.Dispose();
            Container = null;
        }

        public object GetService(Type serviceType)
        {
            return Container.GetInstance(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return Container.GetAllInstances(serviceType).Cast<object>();
        }

        public IMyDependencyScope BeginScope()
        {
            IContainer child = this.Container.GetNestedContainer();
            return new StructureMapDependencyResolver(child);
        }
    }
}
