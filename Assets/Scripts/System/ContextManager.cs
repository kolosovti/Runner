using System;
using System.Collections.Generic;
using UniRx;

namespace Game.System
{
    public abstract class ContextManager : IDisposable
    {
        protected readonly Dictionary<Type, BaseContextController> _controllers =
            new Dictionary<Type, BaseContextController>();

        public readonly CompositeDisposable Subscriptions = new CompositeDisposable();

        public bool IsDisposed { get; private set; }

        public void Init()
        {
            CreateControllers();
            ConnectControllers();
        }

        public void Start()
        {
            InitControllers();
        }

        public void Tick()
        {
            foreach (var controller in _controllers)
                controller.Value.Tick();
        }

        public void FixedTick()
        {
            foreach (var controller in _controllers)
                controller.Value.FixedTick();
        }

        protected abstract void CreateControllers();

        private void InitControllers()
        {
            foreach (var controller in _controllers)
                controller.Value.Init();
        }

        private void ConnectControllers()
        {
            foreach (var controller in _controllers)
                controller.Value.ConnectController();
        }

        public T GetController<T>() where T : BaseContextController
        {
            return _controllers[typeof(T)] as T;
        }

        protected void CreateController<T>(T controller) where T : BaseContextController
        {
            _controllers.Add(typeof(T), controller);
        }

        public void Dispose()
        {
            foreach (var controller in _controllers.Values)
                controller.Dispose();

            _controllers.Clear();
            Subscriptions.Dispose();
            IsDisposed = true;
        }
    }
}