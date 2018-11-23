using Demos.Common.AmbientScopes;

namespace Demos.Common.Ioc.ScopeContexts
{
    public class AmbientScopeContext : IScopeContext
    {
        private readonly IMyDependencyResolver _rootResolver;

        public AmbientScopeContext(IMyDependencyResolver rootResolver)
        {
            _rootResolver = rootResolver;
        }

        public IMyDependencyScope Current
        {
            get
            {
                var current = AmbientScope.Current;
                if (current == null)
                {
                    return _rootResolver.BeginScope();
                }
                if (current.Item == null)
                {
                    current.Item = _rootResolver.BeginScope();
                }
                return current.Item as IMyDependencyScope;
            }
        }
    }
}