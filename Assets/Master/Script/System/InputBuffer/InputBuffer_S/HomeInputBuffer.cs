using UnityEngine;
using UnityEngine.InputSystem;

namespace DCFrameWork.SceneSystem
{
    public class HomeInputBuffer : InputBuffer_B
    {
        protected override void SetAction()
        {
            _moveAction += OnMove;
            _confirm += OnComfirm;
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            _currentContext.MoveInput = context.ReadValue<Vector2>();
            Debug.Log(context.ReadValue<Vector2>());
        }

        private void OnComfirm(InputAction.CallbackContext context)
        {
            _currentContext.Confirm = context.ReadValueAsButton();
        }
    }
}