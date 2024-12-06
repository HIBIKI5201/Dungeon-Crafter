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
                Debug.LogWarning("UIManagerが見つかりませんでした");
            _input = GetComponentInChildren<InputBuffer_B>();
            if (_input is null)
                Debug.LogWarning("InputBufferが見つかりませんでした");

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
        /// Updateで呼ばれます
        /// シーンのマネジメントを行ってください
        /// </summary>
        protected abstract void Think(InputContext input);
    }
}