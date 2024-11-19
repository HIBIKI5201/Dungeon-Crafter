using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

namespace DCFrameWork.SceneSystem
{
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(InputSystemUIInputModule))]
    public abstract class InputBuffer_B : MonoBehaviour
    {
        private PlayerInput _playerInput;

        protected InputContext _currentContext = new();
        private void Start()
        {
            _playerInput = GetComponent<PlayerInput>();
            _playerInput.notificationBehavior = PlayerNotifications.InvokeCSharpEvents;
            _playerInput.camera = Camera.main;
            _playerInput.uiInputModule = GetComponent<InputSystemUIInputModule>();

            Action<InputAction.CallbackContext> action = null;
            if (SetAction(action) is not null)
            {
                _playerInput.onActionTriggered += action;
            }
        }

        protected abstract Action<InputAction.CallbackContext> SetAction(Action<InputAction.CallbackContext> action);

        public InputContext GetContext() => _currentContext;
    }

    public class InputContext
    {
        public Vector2 MoveInput;
        public float RotateInput;
        public Vector2 CameraMoveInput;
        public bool Confirm;
        public bool Pause;
    }
}
