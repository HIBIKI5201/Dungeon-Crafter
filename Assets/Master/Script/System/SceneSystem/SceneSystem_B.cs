using DCFrameWork.UI;
using UnityEngine;
using UnityEngine.Events;

namespace DCFrameWork.SceneSystem
{
    public abstract class SceneSystem_B : MonoBehaviour
    {
        protected InputBuffer_B _input;
        protected UIManager_B _UIManager;

        [SerializeField]
        private UnityEvent _instantiate = new();

        public async void Initialize()
        {
            _UIManager = transform.GetComponentInChildren<UIManager_B>();
            if (_UIManager is null)
                Debug.LogWarning("UIManager��������܂���ł���");
            _input = GetComponentInChildren<InputBuffer_B>();
            if (_input is null)
                Debug.LogWarning("InputBuffer��������܂���ł���");

            Initialize_S();
            _instantiate?.Invoke();

            if (_UIManager is not null)
            {
                await _UIManager.Initialize();
            }
        }

        protected virtual void Initialize_S() { }

        private void Update()
        {
            Think(_input?.GetContext() ?? new());
        }

        /// <summary>
        /// Update�ŌĂ΂�܂�
        /// �V�[���̃}�l�W�����g���s���Ă�������
        /// </summary>
        protected abstract void Think(InputContext input);
    }
}