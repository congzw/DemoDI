using System.Web;

namespace Demos.Common.Ioc.ScopeContexts
{
    public class HttpScopeContext : IScopeContext
    {
        public static string MyDependencyResolverKey = "Common.Ioc.MyDependencyScope";
        private readonly IMyDependencyResolver _rootResolver;
        private readonly HttpContextBase _httpContext;

        public HttpScopeContext(IMyDependencyResolver rootResolver, HttpContextBase httpContext)
        {
            _rootResolver = rootResolver;
            _httpContext = httpContext;
        }

        public IMyDependencyScope Current
        {
            get
            {
                if (_httpContext == null)
                {
                    return _rootResolver.BeginScope();
                }
                if (_httpContext.Items[MyDependencyResolverKey] == null)
                {
                    _httpContext.Items[MyDependencyResolverKey] = _rootResolver.BeginScope();
                }
                return _httpContext.Items[MyDependencyResolverKey] as IMyDependencyScope;
            }
        }
    }
}
