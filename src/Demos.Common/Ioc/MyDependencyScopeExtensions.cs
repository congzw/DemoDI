using System.Collections.Generic;

namespace Demos.Common.Ioc
{
    public static class MyDependencyScopeExtensions
    {
        public static TService GetService<TService>(this IMyDependencyScope scope)
        {
            return (TService)scope.GetService(typeof(TService));
        }

        public static IEnumerable<TService> GetServices<TService>(this IMyDependencyScope scope)
        {
            foreach (object item in scope.GetServices(typeof(TService)))
            {
                yield return (TService)item;
            }
        }
    }
}