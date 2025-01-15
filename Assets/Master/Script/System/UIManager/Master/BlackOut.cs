using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UIElements;

[UxmlElement]
public partial class BlackOut : VisualElement
{
    public Task InitializeTask { get; private set; }
    VisualElement _blackOut;
    public BlackOut() => InitializeTask = Initialize();
    public bool IsBlackOut
    {
        set
        {
            if (value)
            {
                _blackOut.RemoveFromClassList("black-out-false");
                _blackOut.AddToClassList("black-out-true");
                return;
            }
            _blackOut.RemoveFromClassList("black-out-true");
            _blackOut.AddToClassList("black-out-false");
        }
    }
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
            _blackOut = container.Q<VisualElement>("BlackOut");
            _blackOut.RegisterCallback<KeyDownEvent>(e => e.StopImmediatePropagation());
            _blackOut.pickingMode = PickingMode.Ignore;
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
