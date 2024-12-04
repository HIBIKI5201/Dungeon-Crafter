using System;
using System.Threading.Tasks;
using Unity.AppUI.UI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UIElements;

    [UxmlElement]
    public partial class BlackOut : VisualElement
    {
        public Task InitializeTask { get; private set; }
        public BlackOut() => InitializeTask = Initialize();
        public bool IsBlackOut { set { if (value)
            {
                RemoveFromClassList("black-out-false");
                AddToClassList("black-out-true");
                return;
            }
            RemoveFromClassList("black-out-true");
            AddToClassList("black-out-false");
            return;
        } }
        private async Task Initialize()
        {
            AsyncOperationHandle<VisualTreeAsset> handle = Addressables.LoadAssetAsync<VisualTreeAsset>("UXML/BlackOut.uxml");
            await handle.Task;

            if (handle.Status == AsyncOperationStatus.Succeeded && handle.Result != null)
            {
                var treeAsset = handle.Result;
                var container = treeAsset.Instantiate();
                container.style.width = Length.Percent(100);
                container.style.height = Length.Percent(100);
                this.RegisterCallback<KeyDownEvent>(e => e.StopImmediatePropagation());
                pickingMode = PickingMode.Ignore;
                container.RegisterCallback<KeyDownEvent>(e => e.StopImmediatePropagation());
                container.pickingMode = PickingMode.Ignore;
                hierarchy.Add(container);
                //UI要素の取得

                Debug.Log("ウィンドウは正常にロード完了");
            }
            else
            {
                Debug.LogError("Failed to load UXML file from Addressables: UXML/BlackOut.uxml");
            }

            // メモリの解放
            Addressables.Release(handle);
        }
    }
