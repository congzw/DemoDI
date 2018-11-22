using System;
using System.Web;

namespace Demos.Common.Ioc
{
    public class MyDependencyResolverContext
    {
        public IMyDependencyResolver RootResolver { get; set; }
        public MyDependencyResolverContext(IMyDependencyResolver rootResolver)
        {
            RootResolver = rootResolver;
        }
        private IMyDependencyScope GetHttpResolver()
        {
            IMyDependencyScope resolver = RootResolver;
            if (HttpContext.Current == null)
            {
                return resolver;
            }
            
            if (HttpContext.Current.Items["_MyDependencyResolver"] == null)
            {
                resolver =  RootResolver.BeginScope();
            }

            HttpContext.Current.Items["_MyDependencyResolver"] = resolver;
            return resolver;
        }

        //private static Func<IMyDependencyResolver> _httpResolver;

        //public static Func<IMyDependencyResolver> HttpResolver
        //{
        //    get
        //    {
        //        if (_httpResolver == null)
        //        {
        //            _httpResolver = () => CreateHttpResolver();
        //        }
        //        return _httpResolver;
        //    }
        //    set { _httpResolver = value; }
        //}
    }
}
