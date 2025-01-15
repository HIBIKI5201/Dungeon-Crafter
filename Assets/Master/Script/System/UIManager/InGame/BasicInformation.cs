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
    public Task InitializeTask { get; private set; }
    private GuageMesh _guageMesh;
    private VisualElement _eXPGuage;
    private Label _eXPText;
    private Label _phaseText;
    private Button _menuButton;
    public GuageMesh GuageMesh{get => _guageMesh;set => _guageMesh = value;}
    public VisualElement EXPGuage{get => _eXPGuage;set => _eXPGuage = value;}
    public Label EXPText{get => _eXPText;set => _eXPText = value;}
    public Label PhaseText{get => _phaseText;set => _phaseText = value;}
    public Button MenuButton{get => _menuButton;set => _menuButton = value;}
    public BasicInformation() => InitializeTask = Initialize();
    private async Task Initialize()
    {
        AsyncOperationHandle<VisualTreeAsset> handle = Addressables.LoadAssetAsync<VisualTreeAsset>("UXML/BasicInformation.uxml");
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
            _guageMesh = container.Q<GuageMesh>("PhaseGuage");
            _eXPGuage = container.Q<VisualElement>("EXPGuage");
            _eXPText = container.Q<Label>("EXPText");
            _phaseText = container.Q<Label>("PhaseText");
            _menuButton = container.Q<Button>("MenuButton");
        }
        else
        {
            Debug.LogError("Failed to load UXML file from Addressables: UXML/BasicInformation.uxml");
        }
        Addressables.Release(handle);
    }
}