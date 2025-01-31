using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Rendering.FilterWindow;

namespace DCFrameWork
{
    public abstract partial class VisualElement_B : VisualElement
    {
        public Task InitializeTask { get; private set; }
        bool _isInitialized = false;

        public VisualElement_B(string path)
        {
            _isInitialized = false;
            InitializeTask = Initialize(path);
        }

        private async Task Initialize(string path)
        {
            VisualTreeAsset treeAsset = default;
            if (!string.IsNullOrEmpty(path))
            {
                treeAsset = Resources.Load<VisualTreeAsset>(path);
            }
            else
            {
                Debug.LogError($"{name} failed initialize");
                return;
            }


            if (treeAsset != null)
            {
                #region 親エレメントの初期化

                var container = treeAsset.Instantiate();
                container.style.width = Length.Percent(100);
                container.style.height = Length.Percent(100);

                this.RegisterCallback<KeyDownEvent>(e => e.StopPropagation());
                pickingMode = PickingMode.Ignore;
                container.RegisterCallback<KeyDownEvent>(e => e.StopPropagation());
                container.pickingMode = PickingMode.Ignore;

                hierarchy.Add(container);

                #endregion

                // UI要素の取得
                await Initialize_S(container);
                _isInitialized = true;
                Debug.Log("ウィンドウは正常にロード完了");
            }
            else
            {
                Debug.LogError($"Failed to load UXML file from Addressables: {path}");
            }
        }

        protected abstract Task Initialize_S(TemplateContainer container);
    }
}
