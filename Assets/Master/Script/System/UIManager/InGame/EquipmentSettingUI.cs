using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UIElements;

namespace DCFrameWork
{
    [UxmlElement]
    public partial class EquipmentSettingUI : VisualElement_B
    {
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
        public EquipmentSettingUI() : base("UXML/InGame/DefenceEquipmentSetting") { }
        //初期化

        protected override Task Initialize_S(TemplateContainer container)
        {
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

            return Task.CompletedTask;
        }
    }

}

