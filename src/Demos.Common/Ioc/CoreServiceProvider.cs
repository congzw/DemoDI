using System;
using System.Collections.Generic;

namespace Demos.Common.Ioc
{
    /// <summary>
    /// Core Service Locator
    /// </summary>
    public static class CoreServiceProvider
    {
        /// <summary>
        /// 定位服务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T LocateService<T>()
        {
            var service = Current.GetService<T>();
            return service;
        }
        
        /// <summary>
        /// 定位服务列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> LocateServices<T>()
        {
            var services = Current.GetServices<T>();
            return services;
        }

        #region for ioc extensions

        private static Func<IMyDependencyResolver> _currentFunc = () => new Lazy<NullDependencyResolver>(() => new NullDependencyResolver()).Value;
        /// <summary>
        /// 当前实现Factory，支持运行时替换
        /// </summary>
        public static Func<IMyDependencyResolver> CurrentFunc
        {
            get { return _currentFunc; }
            set { _currentFunc = value; }
        }

        /// <summary>
        /// 当前实现
        /// </summary>
        public static IMyDependencyResolver Current
        {
            get { return _currentFunc(); }
        }

        #endregion
    }
}