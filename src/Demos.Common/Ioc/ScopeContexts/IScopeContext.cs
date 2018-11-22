namespace Demos.Common.Ioc.ScopeContexts
{
    public interface IScopeContext
    {
        IMyDependencyScope Current { get; }
    }
}