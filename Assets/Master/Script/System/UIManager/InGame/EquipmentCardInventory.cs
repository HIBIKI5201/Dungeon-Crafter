using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UIElements;

namespace DCFrameWork
{
    [UxmlElement]
    public partial class EquipmentCardInventory : VisualElement
    {
        //初期化タスク
        public Task InitializeTask { get; private set; }
        //定数
        private const string _windowClose = "equipment-inventory_close";
        private const string _windowOpen = "equipment-inventory_open";

        //UI要素
        private VisualElement _equipment;
        private VisualElement _equipmentButton;
        private VisualElement _backGround;
        //タレット設置担当用マウスカーソルが乗っているとき離れたときに発火するイベント
        public event Action<bool> OnMouseCursor;
        public EquipmentCardInventory() => InitializeTask = Initialize();
        // 初期化
        private async Task Initialize()
        {
            //UXMLファイルの読み込み
            AsyncOperationHandle<VisualTreeAsset> handle = Addressables.LoadAssetAsync<VisualTreeAsset>("UXML/EquipmentInventory.uxml");
            await handle.Task;

            if (handle.Status == AsyncOperationStatus.Succeeded && handle.Result != null)
            {
                //UXMLファイルの読み込み
                var treeAsset = handle.Result;
                var container = treeAsset.Instantiate();
                //スタイルの読み込み
                container.style.width = Length.Percent(100);
                container.style.height = Length.Percent(100);
                hierarchy.Add(container);
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
                _equipment = container.Q<VisualElement>("EquipsInventory");
                _equipmentButton = container.Q<VisualElement>("EquipmentTextBox");
                _backGround = container.Q<VisualElement>("Background");
                //UIがマウスカーソルが上に乗った時のイベント発火
                _equipment.RegisterCallback<MouseEnterEvent>(x=>OnMouseCursor?.Invoke(true));
                _equipment.RegisterCallback<MouseLeaveEvent>(x=>OnMouseCursor?.Invoke(false));                
                _equipmentButton.RegisterCallback<MouseEnterEvent>(x=>OnMouseCursor?.Invoke(true));
                _equipmentButton.RegisterCallback<MouseLeaveEvent>(x=>OnMouseCursor?.Invoke(false));
                //スタイルの読み込み
                _equipment.AddToClassList(_windowClose);
                _equipmentButton.RegisterCallback<ClickEvent>(x =>
                {
                    if (!_equipment.ClassListContains(_windowClose)) return;
                    _equipment.RemoveFromClassList(_windowClose);
                    _equipment.AddToClassList(_windowOpen);
                });
                _backGround.RegisterCallback<ClickEvent>(x =>
                {
                    if (!_equipment.ClassListContains(_windowOpen)) return;
                    _equipment.RemoveFromClassList(_windowOpen);
                    _equipment.AddToClassList(_windowClose);
                });
                Debug.Log("�E�B���h�E�͐���Ƀ��[�h����");
            }
            else
            {
                Debug.LogError("Failed to load UXML file from Addressables: UXML/EquipmentInventory.uxml");
            }

            Addressables.Release(handle);
        }
    }
}
