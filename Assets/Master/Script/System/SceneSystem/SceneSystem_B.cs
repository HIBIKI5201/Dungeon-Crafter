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
                Debug.LogWarning("UIManagerが見つかりませんでした");
            _input = GetComponentInChildren<InputBuffer_B>();
            if (_input is null)
                Debug.LogWarning("InputBufferが見つかりませんでした");

            await _UIManager.Initialize();

            Initialize_S();
        }

        protected virtual void Initialize_S() { }

        private void Update()
        {
            Think(_input?.GetContext() ?? new());
        }

        /// <summary>
        /// Updateで呼ばれます
        /// シーンのマネジメントを行ってください
        /// </summary>
        protected abstract void Think(InputContext input);
    }
}