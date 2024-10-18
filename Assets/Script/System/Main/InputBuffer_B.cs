using UnityEngine;
using UnityEngine.InputSystem;


namespace DCFrameWork.InputBuffer
{
    [RequireComponent(typeof(PlayerInput))]
    public abstract class InputBuffer_B : MonoBehaviour
    {
        PlayerInput _playerInput;

        InputContext _context;
        private void Start()
        {
            _playerInput = GetComponent<PlayerInput>();
            _context = new();
        }

        private void Update()
        {
            _context.MoveInput = _playerInput.actions["Move"].ReadValue<Vector2>();
            _context.CameraInput = _playerInput.actions["Look"].ReadValue<Vector2>();
            _context.Comfirm = _playerInput.actions["Fire"].triggered;
        }

        private void LateUpdate()
        {
            _context.ContextReset();
        }

        public InputContext GetContext() => _context;
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
