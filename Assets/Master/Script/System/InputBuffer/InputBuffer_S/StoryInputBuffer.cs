using System;
using UnityEngine.InputSystem;

namespace DCFrameWork.SceneSystem
{
    public class StoryInputBuffer : InputBuffer_B
    {
        protected override bool SetAction(ref Action<InputAction.CallbackContext> action)
        {
            return action is not null;
        }
    }
}
