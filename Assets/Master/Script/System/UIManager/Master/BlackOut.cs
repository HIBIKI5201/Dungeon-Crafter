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
    //コンストラクタ
    public BlackOut() => InitializeTask = Initialize();
    //プロパティ
    public bool IsBlackOut
    {
        set
        {
            if (value)
            {
                //ウィンドウを閉じる
                _blackOut.RemoveFromClassList("black-out-false");
                _blackOut.AddToClassList("black-out-true");
                return;
            }
            //ウィンドウを開く
            _blackOut.RemoveFromClassList("black-out-true");
            _blackOut.AddToClassList("black-out-false");
        }
    }
    private async Task Initialize()
    {
        //UXMLファイルの読み込み
        AsyncOperationHandle<VisualTreeAsset> handle = Addressables.LoadAssetAsync<VisualTreeAsset>("UXML/BlackOut.uxml");
        await handle.Task;

        if (handle.Status == AsyncOperationStatus.Succeeded && handle.Result != null)
        {
            //UXMLファイルの読み込み
            var treeAsset = handle.Result;
            var container = treeAsset.Instantiate();
            //スタイルの読み込み
            container.style.width = Length.Percent(100);
            container.style.height = Length.Percent(100);
            //マウスイベントの無効化
            this.RegisterCallback<KeyDownEvent>(e => e.StopImmediatePropagation());
            pickingMode = PickingMode.Ignore;
            container.RegisterCallback<KeyDownEvent>(e => e.StopImmediatePropagation());
            container.pickingMode = PickingMode.Ignore;
            //UI要素の取得
            _blackOut = container.Q<VisualElement>("BlackOut");
            _blackOut.RegisterCallback<KeyDownEvent>(e => e.StopImmediatePropagation());
            _blackOut.pickingMode = PickingMode.Ignore;
            hierarchy.Add(container);
        }
        else
        {
            Debug.LogError("Failed to load UXML file from Addressables: UXML/BlackOut.uxml");
        }
        Addressables.Release(handle);
    }
}
