using System;
using UniRx;

namespace Game.System
{
    public abstract class BaseContextController : IDisposable
    {
        protected readonly CompositeDisposable _subscriptions = new CompositeDisposable();

        public readonly ContextManager ContextManager;

        protected BaseContextController(ContextManager contextManager)
        {
            ContextManager = contextManager;
        }

        public void Init()
        {
            SelfInit();
        }

        public T GetController<T>() where T : BaseContextController
        {
            return ContextManager.GetController<T>();
        }

        public virtual void Tick()
        {
        }

        protected virtual void SelfInit()
        {
        }

        public virtual void ConnectController()
        {
        }

        public virtual void Dispose()
        {
            _subscriptions.Dispose();
        }
    }
}