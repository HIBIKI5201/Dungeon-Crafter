using UnityEngine;
using UnityEngine.InputSystem;

namespace DCFrameWork.SceneSystem
{
    public class InGameInputBuffer : InputBuffer_B
    {
        protected override void SetAction()
        {
            _moveAction += OnMove;
            _rotateAction += OnRotate;
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            _currentContext.MoveInput = context.ReadValue<Vector2>();
        }

        private void OnRotate(InputAction.CallbackContext context)
        {
            _currentContext.RotateInput = context.ReadValue<float>();
        }
    }
}