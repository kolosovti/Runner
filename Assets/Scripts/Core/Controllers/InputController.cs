using Game.System;
using UnityEngine;

namespace Game.Core.Controllers
{
    public class InputController : BaseContextController
    {
        private readonly InputModel _inputModel;

        public InputController(InputModel inputModel, ContextManager contextManager) : base(contextManager)
        {
            _inputModel = inputModel;
        }
        
        public override void Tick()
        {
            base.Tick();
            
            if (Input.GetMouseButtonDown(0))
            {
                _inputModel.InvokeJump();
            }
        }
    }
}