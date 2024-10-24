using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;


namespace DCFrameWork.InputBuffer
{
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(InputSystemUIInputModule))]
    public abstract class InputBuffer_B : MonoBehaviour
    {
        private PlayerInput _playerInput;

        protected Action<InputAction.CallbackContext> _moveAction;
        protected Action<InputAction.CallbackContext> _cameraAction;
        protected Action<InputAction.CallbackContext> _comfirm;
        protected Action<InputAction.CallbackContext> _pause;

        private InputContext _currentContext = new();
        private void Start()
        {
            _playerInput = GetComponent<PlayerInput>();
            _playerInput.notificationBehavior = PlayerNotifications.InvokeCSharpEvents;
            _playerInput.uiInputModule = GetComponent<InputSystemUIInputModule>();

            InputActionMap map = _playerInput.currentActionMap;
            map.FindAction("Move").performed += cbc => _moveAction?.Invoke(cbc);
            map.FindAction("Look").performed += cbc => _cameraAction?.Invoke(cbc);
            map.FindAction("Fire").performed += cbc => _comfirm?.Invoke(cbc);
        }

        private void LateUpdate()
        {
            _currentContext = new();
        }

        public InputContext GetContext() => _currentContext;

        protected virtual void OnAction(InputAction.CallbackContext context)
        {
            switch (context.action.name)
            {
                case "Move":
                    _currentContext.MoveInput = context.ReadValue<Vector2>();
                    break;

                case "Look":
                    _currentContext.CameraInput = context.ReadValue<Vector2>();
                    break;

                case "Fire":
                    _currentContext.Comfirm = true;
                    break;
            }
        }
    }

    public struct InputContext
    {
        public Vector2 MoveInput;
        public Vector2 CameraInput;
        public bool Comfirm;
        public bool Pause;
    }
}
