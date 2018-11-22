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

        //����CA1063�Ľ��飬��дDispose
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
            //�����ڲ�û�з�װ�κ��йܻ���й���Դ������������⴦��ֱ��ά����ȷ�������ṹ����
            if (!_disposed)
            {
                if (disposing)
                {
                    //�����й���Դ
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
                //������й���Դ
            }
            _disposed = true;
        }

    }
}