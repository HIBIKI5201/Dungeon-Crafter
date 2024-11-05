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
        protected Action<InputAction.CallbackContext> _rotateAction = null;
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

            _playerInput.onActionTriggered += MoveAction;
            _playerInput.onActionTriggered += RotateAction;
            _playerInput.onActionTriggered += LookAction;
            _playerInput.onActionTriggered += FireAction;
        }

        private void MoveAction(InputAction.CallbackContext context) { if (context.action.name is "Move") _moveAction?.Invoke(context);}
        private void RotateAction(InputAction.CallbackContext context) { if (context.action.name is "Rotate") _rotateAction?.Invoke(context); }
        private void LookAction(InputAction.CallbackContext context) { if (context.action.name is "Look") _cameraAction?.Invoke(context); }
        private void FireAction(InputAction.CallbackContext context) { if (context.action.name is "Fire") _confirm?.Invoke(context); }

    private void OnDisable()
        {
            _playerInput.onActionTriggered -= MoveAction;
            _playerInput.onActionTriggered -= RotateAction;
            _playerInput.onActionTriggered -= LookAction;
            _playerInput.onActionTriggered -= FireAction;

        }
        protected abstract void SetAction();

        public InputContext GetContext() => _currentContext;
    }

    public struct InputContext
    {
        public Vector2 MoveInput;
        public float RotateInput;
        public Vector2 CameraMoveInput;
        public bool Confirm;
        public bool Pause;
    }
}
