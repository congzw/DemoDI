using System;
using System.Collections.Concurrent;
using System.Runtime.Remoting.Messaging;

namespace Demos.Common.Ioc.ScopeContexts
{
    public class AmbientScope : IDisposable
    {
        public IDisposable Item { get; set; }

        private static readonly string _scopeStackKey = "Common.Ioc.AmbientScopeStack";
        internal static ConcurrentStack<AmbientScope> ScopeStack
        {
            get
            {
                return CallContext.LogicalGetData(_scopeStackKey) as ConcurrentStack<AmbientScope>;
            }
            set
            {
                CallContext.LogicalSetData(_scopeStackKey, value);
            }
        }

        public AmbientScope()
        {
            if (ScopeStack == null)
            {
                ScopeStack = new ConcurrentStack<AmbientScope>();
            }
            ScopeStack.Push(this);
        }

        public static AmbientScope Current
        {
            get
            {
                if (ScopeStack == null || ScopeStack.IsEmpty)
                {
                    return null;
                }
                AmbientScope result;
                ScopeStack.TryPeek(out result);
                return result;
            }
        }

        //根据CA1063的建议，重写Dispose
        //https://msdn.microsoft.com/query/dev12.query?appId=Dev12IDEF1&l=ZH-CN&k=k(CA1063);k(TargetFrameworkMoniker-.NETFramework,Version%3Dv4.5)&rd=true
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        ~AmbientScope()
        {
            Dispose(false);
        }
        private bool _disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            //本类内部没有封装任何托管或非托管资源，所以无需额外处理，直接维护正确的容器结构即可
            if (!_disposed)
            {
                if (disposing)
                {
                    //处理托管资源
                    if (Item != null)
                    {
                        Item.Dispose();
                        Item = null;
                    }

                    if (!ScopeStack.IsEmpty)
                    {
                        AmbientScope result;
                        ScopeStack.TryPop(out result);
                    }
                    if (ScopeStack.IsEmpty)
                    {
                        CallContext.FreeNamedDataSlot(_scopeStackKey);
                    }
                }
                //处理非托管资源
            }
            _disposed = true;
        }

    }
}