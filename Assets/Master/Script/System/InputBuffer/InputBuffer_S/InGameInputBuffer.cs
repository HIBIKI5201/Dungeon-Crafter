using UnityEngine;
using DCFrameWork.InputBuffer;
using UnityEngine.InputSystem;

public class InGameInputBuffer : InputBuffer_B
{
    protected override void SetAction()
    {
        _moveAction += OnMove;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        _currentContext.MoveInput = context.ReadValue<Vector2>();
    }
}
