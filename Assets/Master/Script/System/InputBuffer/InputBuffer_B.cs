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

        protected Action<InputAction.CallbackContext> _moveAction = null;
        protected Action<InputAction.CallbackContext> _cameraAction = null;
        protected Action<InputAction.CallbackContext> _confirm = null;
        protected Action<InputAction.CallbackContext> _pause = null;

        protected InputContext _currentContext = new();
        private void Start()
        {
            _playerInput = GetComponent<PlayerInput>();
            _playerInput.notificationBehavior = PlayerNotifications.InvokeCSharpEvents;
            _playerInput.camera = Camera.main;
            _playerInput.uiInputModule = GetComponent<InputSystemUIInputModule>();

            SetAction();

            _playerInput.onActionTriggered += cbc => { if (cbc.action.name is "Move") _moveAction?.Invoke(cbc); };
            _playerInput.onActionTriggered += cbc => { if (cbc.action.name is "Look") _cameraAction?.Invoke(cbc); };
            _playerInput.onActionTriggered += cbc => { if (cbc.action.name is "Fire") _confirm?.Invoke(cbc); };
        }

        protected abstract void SetAction();

        public InputContext GetContext() => _currentContext;
    }

    public struct InputContext
    {
        public Vector2 MoveInput;
        public Vector2 CameraMoveInput;
        public int RotateInput;
        public bool Confirm;
        public bool Pause;
    }
}
