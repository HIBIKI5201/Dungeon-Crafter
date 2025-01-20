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
        //初期化タスク
        public Task InitializeTask { get; private set; }
        //UI要素
        VisualElement _equipmentSettingWindow;
        Button _powerUpButton;
        Button _removalButton;
        Action _powerUpCallback;
        Action _removalCallback;
        Label _equipmentName;
        Label _levelText;
        Label _powerText;
        Label _fastText;
        Label _rangeText;
        //プロパティ
        public bool EquipmentSettingWindowVisible 
        {
        set { 
                if (value)
                {
                    //ウィンドウを閉じる
                    _equipmentSettingWindow.RemoveFromClassList("equipment-setting-window-open");
                    _equipmentSettingWindow.AddToClassList("equipment-setting-window-close");
                    return;
                }
                //ウィンドウを開く
                _equipmentSettingWindow.RemoveFromClassList("equipment-setting-window-close");
                _equipmentSettingWindow.AddToClassList("equipment-setting-window-open");
            } 
        }
        public Action PowerUpButton
        { 
            set
            {
                _powerUpButton.clicked -= _powerUpCallback;
                _powerUpCallback = value;
                _powerUpButton.clicked += _powerUpCallback;
            }
        }
        public Action RemovalButton
        { 
            set
            {
                _removalButton.clicked -= _removalCallback;
                _removalCallback = value;
                _removalButton.clicked += _removalCallback;
            }
        }
        public string EquipmentName { set => _equipmentName.text = value; }
        public string LevelText { set => _levelText.text = value; }
        public string PowerText { set => _powerText.text = value; }
        public string FastText { set => _fastText.text = value; }
        public string RangeText { set => _rangeText.text = value; }
        public string PowerUpButtonText{set => _powerUpButton.text = value;}
        public string RemovalButtonName{ set => _removalButton.text = value;}
        //マウスがUiの上の乗った時に発火するイベント
        public event Action<bool> OnMouseEvent;
        //  コンストラクタ
        public EquipmentSettingUI() => InitializeTask = Initialize();
        //初期化
        private async Task Initialize()
        {
            //UXMLファイルの読み込み
            AsyncOperationHandle<VisualTreeAsset> handle = Addressables.LoadAssetAsync<VisualTreeAsset>("UXML/DefenceEquipmentSetting.uxml");
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
                _powerUpButton = container.Q<Button>("PowerUpButton");
                _removalButton = container.Q<Button>("RemovalButton");
                _equipmentName = container.Q<Label>("EquipmentName");
                _levelText = container.Q<Label>("LevelText");
                _powerText = container.Q<Label>("PowerText");
                _fastText = container.Q<Label>("FastText");
                _rangeText = container.Q<Label>("RangeText");
                _equipmentSettingWindow = container.Q<VisualElement>("EquipmentSettingWindow");
                //UIの上にマウスが乗った時のイベント発火
                _equipmentSettingWindow.RegisterCallback<MouseEnterEvent>(x => OnMouseEvent?.Invoke(true));
                _equipmentSettingWindow.RegisterCallback<MouseLeaveEvent>(x => OnMouseEvent?.Invoke(false));
                _equipmentSettingWindow.AddToClassList("equipment-setting-window-close");
            }
            else
            {
                Debug.LogError("Failed to load UXML file from Addressables: UXML / DefenceEquipmentSetting.uxml");
            }
            Addressables.Release(handle);
        }
    }

}

