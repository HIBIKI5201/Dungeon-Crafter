using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DCFrameWork.SceneSystem
{
    public class HomeInputBuffer : InputBuffer_B
    {
        protected override bool SetAction(ref Action<InputAction.CallbackContext> action)
        {
            return action is not null;
        }
    }
}