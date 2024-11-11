using DCFrameWork.MainSystem;
using UnityEngine;

namespace DCFrameWork.SceneSystem
{
    public abstract class SceneSystem_B : MonoBehaviour
    {
        protected InputBuffer_B _input;
        protected UIManager_B _UIManager;

        public async void Initialize()
        {
            _UIManager = transform.GetComponentInChildren<UIManager_B>();
            if (_UIManager is null)
                Debug.LogWarning("UIManager��������܂���ł���");
            _input = GetComponentInChildren<InputBuffer_B>();
            if (_input is null)
                Debug.LogWarning("InputBuffer��������܂���ł���");

            await _UIManager.Initialize();

            Initialize_S();
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