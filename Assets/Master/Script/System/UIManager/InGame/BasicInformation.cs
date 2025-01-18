using System;
using System.Threading.Tasks;
using DCFrameWork;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UIElements;

[UxmlElement]
public partial class BasicInformation : VisualElement
{
    //初期化タスク
    public Task InitializeTask { get; private set; }
    //UI要素
    private GuageMesh _guageMesh;
    private VisualElement _eXPGuage;
    private Label _eXPText;
    private Label _phaseText;
    private Label _goldText;

    private Button _menuButton;
    private float _exp;
    private float _level;
    //プロパティ
    public float Exp
    {
        set
        {
            _exp = value;
            EXPTextUpdate();
        }
    }
    public float Level
    {
        set
        {
            _level = value;
            EXPTextUpdate();
        }
    }
    public float Money
    {
        set
        {
            _goldText.text = "所持金:" + value;
        }
    }
    public GuageMesh GuageMesh { get => _guageMesh; set => _guageMesh = value; }
    public VisualElement EXPGuage { get => _eXPGuage; set => _eXPGuage = value; }
    public string EXPText { get => _eXPText.text; set => _eXPText.text = value; }
    public string PhaseText { get => _phaseText.text; set => _phaseText.text = value; }
    public Button MenuButton { get => _menuButton; set => _menuButton = value; }
    public BasicInformation() => InitializeTask = Initialize();
    public void EXPTextUpdate()
    {
        string expText = "EXP:" + _exp.ToString() + Environment.NewLine + "Level:" + _level.ToString();
        EXPText = expText;
    }
    public void EXPGuageUpdate(float persentNormal)
    {
        float persent = persentNormal / 100f * 87;
        _eXPGuage.style.width = Length.Percent(persent);
    }
    //初期化
    private async Task Initialize()
    {
        AsyncOperationHandle<VisualTreeAsset> handle = Addressables.LoadAssetAsync<VisualTreeAsset>("UXML/BasicInformation.uxml");
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
            hierarchy.Add(container);
            //UI要素の取得
            _guageMesh = container.Q<GuageMesh>("PhaseGuage");
            _eXPGuage = container.Q<VisualElement>("EXPGuage");
            _eXPText = container.Q<Label>("EXPText");
            _phaseText = container.Q<Label>("PhaseText");
            _menuButton = container.Q<Button>("MenuButton");
            _goldText = container.Q<Label>("MoneyText");
        }
        else
        {
            //エラーログ
            Debug.LogError("Failed to load UXML file from Addressables: UXML/BasicInformation.uxml");
        }
        //リソースの解放
        Addressables.Release(handle);
    }
}