using System;

namespace Demos.Common.Ioc.Impls.Mock
{
    public abstract class MockBase : IDisposable
    {
        public string CreatedBy { get; set; }

        public void Dispose()
        {
            string message = string.Format("Disposed => <{0}:{1}>", this.GetType().Name, this.GetHashCode());
            AssertHelper.WriteLine(message);
        }
    }

    public class MockPerRequest : MockBase
    {
    }

    public class MockPerSession : MockBase
    {
    }

    public class MockPerSingleton : MockBase
    {
    }

    public class MockUow : IDisposable
    {
        public MockUow()
        {
            this.HashCode = this.GetHashCode().ToString();
        }

        public string HashCode { get; set; }

        public void Dispose()
        {
        }
    }
}
