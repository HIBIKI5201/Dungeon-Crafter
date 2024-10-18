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
        PlayerInput _playerInput;

        InputContext _context;
        private void Start()
        {
            _playerInput = GetComponent<PlayerInput>();
            _playerInput.notificationBehavior = PlayerNotifications.InvokeCSharpEvents;
            _playerInput.uiInputModule = GetComponent<InputSystemUIInputModule>();
            _context = new();

            _playerInput.onActionTriggered += OnAction;
        }

        private void LateUpdate()
        {
            _context.ContextReset();
        }

        public InputContext GetContext() => _context;

        protected virtual void OnAction(InputAction.CallbackContext context)
        {
            switch (context.action.name)
            {
                case "Move":
                    _context.MoveInput = context.ReadValue<Vector2>();
                    break;

                case "Look":
                    _context.CameraInput = context.ReadValue<Vector2>();
                    break;
                
                case "Fire":
                    _context.Comfirm = true;
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

        public void ContextReset()
        {
            MoveInput = Vector2.zero;
            CameraInput = Vector2.zero;
            Comfirm = false;
            Pause = false;
        }
    }
}
