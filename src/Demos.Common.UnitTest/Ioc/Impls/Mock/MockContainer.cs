using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Demos.Common.Ioc.Impls.Mock
{
    public class MockContainer : IDisposable
    {
        private readonly Func<MockUow> _uowFunc;

        public MockContainer(Func<MockUow> uowFunc)
        {
            _uowFunc = uowFunc;
        }

        public string Name {
            get
            {
                if (Parent != null)
                {
                    return Parent.Name + "." + "Child";
                }
                return "Default";
            }
        }
        public MockContainer Parent { get; set; }

        private MockPerSingleton _singleton = null;
        public MockPerSingleton Singleton()
        {
            if (Parent != null)
            {
                return Parent.Singleton();
            }
            return _singleton ?? (_singleton = new MockPerSingleton());
        }

        private readonly IDictionary<string, MockPerSession> _Sessions = new ConcurrentDictionary<string, MockPerSession>();
        public MockPerSession Session()
        {
            if (Parent != null)
            {
                return Parent.Session();
            }
            var uow = _uowFunc();
            if (!_Sessions.ContainsKey(uow.HashCode))
            {
                _Sessions[uow.HashCode] = new MockPerSession();
            }
            return _Sessions[uow.HashCode];
        }

        private readonly IList<MockPerRequest> _Requests = new List<MockPerRequest>();
        public MockPerRequest Request()
        {
            var mockPerRequest = new MockPerRequest();
            _Requests.Add(mockPerRequest);
            return mockPerRequest;
        }

        public void Dispose()
        {
            foreach (var mockPerRequest in _Requests)
            {
                mockPerRequest.Dispose();
            }
            _Requests.Clear();
            
            var mockPerSessions = _Sessions.Values.ToList();
            foreach (var session in mockPerSessions)
            {
                session.Dispose();
            }
            _Sessions.Clear();

            if (_singleton != null)
            {
                _singleton.Dispose();
                _singleton = null;
            }

            //string message = string.Format("Container Disposed => <{0}:{1}>", this.GetType().Name, this.GetHashCode());
            //AssertHelper.WriteLine(message);
        }
    }

    public class MockDependencyResolver : IMyDependencyResolver
    {
        private readonly Func<MockContainer> _containerFunc;
        private MockContainer _container = null;

        public MockDependencyResolver(Func<MockContainer> containerFunc)
        {
            _containerFunc = containerFunc;
            _container = containerFunc();
        }

        public object GetService(Type serviceType)
        {
            if (typeof(MockPerSingleton) == serviceType)
            {
                var instance = _container.Singleton();
                instance.CreatedBy = _container.Name;
                return instance;
            }
            if (typeof(MockPerSession) == serviceType)
            {
                var instance = _container.Session();
                instance.CreatedBy = _container.Name;
                return instance;
            }
            if (typeof(MockPerRequest) == serviceType)
            {
                var instance = _container.Request();
                instance.CreatedBy = _container.Name;
                return instance;
            }
            return null;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return Enumerable.Empty<object>();
        }

        public IMyDependencyScope BeginScope()
        {
            var childContainer = _containerFunc();
            childContainer.Parent = _container;
            return new MockDependencyResolver(() => childContainer) { Parent = this };
        }

        public void Dispose()
        {
            Parent = null;
            _container.Dispose();
            _container = null;

            //string message = string.Format("MockDependencyResolver Disposed => <{0}:{1}>", this.GetType().Name, this.GetHashCode());
            //AssertHelper.WriteLine(message);
        }

        public IMyDependencyScope Parent { get; set; }
    }
}
