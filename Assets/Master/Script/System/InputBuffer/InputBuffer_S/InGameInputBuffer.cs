using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DCFrameWork.SceneSystem
{
    public class InGameInputBuffer : InputBuffer_B
    {
        protected override bool SetAction( ref Action<InputAction.CallbackContext> action)
        {
            action += OnMove;
            action += OnRotate;
            return action is not null;
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            if (context.action.name is "Move")
                _currentContext.MoveInput = context.ReadValue<Vector2>();
        }

        private void OnRotate(InputAction.CallbackContext context)
        {
            if (context.action.name is "Rotate")
                _currentContext.RotateInput = context.ReadValue<float>();
        }
    }
}