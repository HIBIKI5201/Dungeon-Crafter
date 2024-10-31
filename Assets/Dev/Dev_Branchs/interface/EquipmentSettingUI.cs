using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UIElements;

namespace DCFrameWork
{
    [UxmlElement]
    public partial class EquipmentSettingUI : VisualElement
    {
        public Task InitializeTask { get; private set; }

        Button _powerUpButton;
        Button _removalButton;
        Label _equipmentName;
        Label _powerUpGText;
        Label _levelText;
        Label _powerText;
        Label _fastText;
        Label _rangeText;
        public Action PowerUpButton{ set => _powerUpButton.clicked += value; }
        public Action RemovalButton { set => _removalButton.clicked += value; }
        public string EquipmentName { set => _equipmentName.text = value; }
        public string PowerUpGText { set => _powerUpGText.text = value; }
        public string LevelText { set => _levelText.text = value; }
        public string PowerText { set => _powerText.text = value; }
        public string FastText { set => _fastText.text = value; }
        public string RangeText { set => _rangeText.text = value; }
        public EquipmentSettingUI() => InitializeTask = Initialize();
        private async Task Initialize()
        {
            AsyncOperationHandle<VisualTreeAsset> handle = Addressables.LoadAssetAsync<VisualTreeAsset>("UXML/DefenceEquipmentSetting.uxml");
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
                _powerUpButton = container.Q<Button>("PowerUpButton");
                _removalButton = container.Q<Button>("RemovalButton");
                _equipmentName = container.Q<Label>("EquipmentName");
                _powerUpGText = container.Q<Label>("PowerUpGText");
                _levelText = container.Q<Label>("LavelText");
                _powerText = container.Q<Label>("PowerText");
                _fastText = container.Q<Label>("FastText");
                _rangeText = container.Q<Label>("RangeText");
                Debug.Log("ウィンドウは正常にロード完了");
            }
            else
            {
                Debug.LogError("Failed to load UXML file from Addressables: UXML / DefenceEquipmentSetting.uxml");
            }
            Addressables.Release(handle);
        }
    }

}

