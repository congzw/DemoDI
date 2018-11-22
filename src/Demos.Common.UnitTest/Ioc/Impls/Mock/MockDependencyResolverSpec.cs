//using Microsoft.VisualStudio.TestTools.UnitTesting;

//namespace Demos.Common.Ioc.Impls.Mock
//{
//    [TestClass]
//    public class MockDependencyResolverSpec
//    {
//        [TestMethod]
//        public void Singleton_Should_Same()
//        {
//            using (var myDependencyScope = new MockDependencyResolver(() => new MockContainer(() => new MockUow())))
//            {
//                var singleton = myDependencyScope.GetService<MockPerSingleton>();
//                singleton.CreatedBy.ShouldEqual("Default");

//                var singleton2 = myDependencyScope.GetService<MockPerSingleton>();
//                singleton2.CreatedBy.ShouldEqual("Default");

//                singleton2.ShouldSame(singleton);
//            }
//        }

//        [TestMethod]
//        public void Singleton_Nested_Should_Same()
//        {
//            using (var myDependencyScope = new MockDependencyResolver(() => new MockContainer(() => new MockUow())))
//            {
//                var singleton = myDependencyScope.GetService<MockPerSingleton>();
//                singleton.CreatedBy.ShouldEqual("Default");
//                //singleton.LogHashCode();

//                using (var childScope = myDependencyScope.BeginScope())
//                {
//                    var child = childScope.GetService<MockPerSingleton>();
//                    child.CreatedBy.ShouldEqual("Default.Child");
//                    //child.LogHashCode();
//                    child.ShouldSame(singleton);
//                }
//            }
//        }

//        [TestMethod]
//        public void Session_InSameUowScope_Should_Same()
//        {
//            var mockUow = new MockUow();
//            using (var myDependencyScope = new MockDependencyResolver(() => new MockContainer(() => mockUow)))
//            {
//                var session = myDependencyScope.GetService<MockPerSession>();
//                session.CreatedBy.ShouldEqual("Default");

//                var session2 = myDependencyScope.GetService<MockPerSession>();
//                session2.CreatedBy.ShouldEqual("Default");

//                session2.ShouldSame(session);
//            }
//        }

//        [TestMethod]
//        public void Session_InDiffUowScope_Should_Diff()
//        {
//            var mockUow = new MockUow();
//            using (var myDependencyScope = new MockDependencyResolver(() => new MockContainer(() => mockUow)))
//            {
//                var session = myDependencyScope.GetService<MockPerSession>();
//                session.CreatedBy.ShouldEqual("Default");

//                mockUow = new MockUow();
//                var session2 = myDependencyScope.GetService<MockPerSession>();
//                session2.CreatedBy.ShouldEqual("Default");

//                session2.ShouldNotSame(session);
//            }
//        }
        
//        [TestMethod]
//        public void Request_Should_Diff()
//        {
//            using (var myDependencyScope = new MockDependencyResolver(() => new MockContainer(() => new MockUow())))
//            {
//                var request = myDependencyScope.GetService<MockPerRequest>();
//                request.CreatedBy.ShouldEqual("Default");

//                var request2 = myDependencyScope.GetService<MockPerRequest>();
//                request2.CreatedBy.ShouldEqual("Default");

//                request2.ShouldNotSame(request);
//            }
//        }

//        [TestMethod]
//        public void Request_Nested_Should_Diff()
//        {
//            using (var myDependencyScope = new MockDependencyResolver(() => new MockContainer(() => new MockUow())))
//            {
//                var request = myDependencyScope.GetService<MockPerRequest>();
//                request.CreatedBy.ShouldEqual("Default");

//                using (var childScope = myDependencyScope.BeginScope())
//                {
//                    var request2 = childScope.GetService<MockPerRequest>();
//                    request2.CreatedBy.ShouldEqual("Default.Child");
//                    request2.ShouldNotSame(request);
//                }
//            }
//        }
//    }
//}
