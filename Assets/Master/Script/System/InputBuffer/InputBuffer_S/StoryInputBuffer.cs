using System;
using UnityEngine.InputSystem;

namespace DCFrameWork.SceneSystem
{
    public class StoryInputBuffer : InputBuffer_B
    {
        protected override Action<InputAction.CallbackContext> SetAction(Action<InputAction.CallbackContext> action)
        {
            return action;
        }
    }
}
