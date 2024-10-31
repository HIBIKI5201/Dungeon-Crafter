using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UIElements;

[UxmlElement]
public partial class BasicInformation : VisualElement
{
    Button _menu;
    VisualElement _waveGuege;
    VisualElement _expGuege;
    Label _waveText;
    Label _expText;
    Label _goldText;
    Label _levelText;
    Label _killEnemyCountText;
    Label _equipmentText;
    public Action MenuButton { set => _menu.clicked += value; }
    public float WaveGuege { set => _waveGuege.style.width = Length.Percent(value * 100); }
    public float EXPGuege { set => _expGuege.style.width = Length.Percent(value * 100); }
    public string WaveText {set=>_waveText.text = value; }
    public string EXPText { set => _expText.text = value; }
    public string GoldText { set => _goldText.text = value; }
    public string LevelText { set => _levelText.text = value; }
    public string KillEnemyCountText { set => _killEnemyCountText.text = value; }
    public string EquipmentText { set => _equipmentText.text = value; }
    public BasicInformation() => Initialize();
    private async void Initialize()
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

            // UI要素の取得
            _menu = container.Q<Button>("MenuButton");
            _waveGuege = container.Q<VisualElement>("WaveGuege");
            _expGuege = container.Q<VisualElement>("EXPGuege");
            _waveText = container.Q<Label>("WaveText");
            _expText = container.Q<Label>("EXPText");
            _goldText = container.Q<Label>("GoldText");
            _levelText = container.Q<Label>("LevelText");
            _killEnemyCountText = container.Q<Label>("KillEnemyCountText");
            _equipmentText = container.Q<Label>("EquipmentText");

            Debug.Log("ウィンドウは正常にロード完了");
        }
        else
        {
            Debug.LogError("Failed to load UXML file from Addressables: UXML/BasicInformation.uxml");
        }

        // メモリの解放
        Addressables.Release(handle);
    }
}