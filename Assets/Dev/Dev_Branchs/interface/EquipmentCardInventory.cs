using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UIElements;

namespace DCFrameWork
{
    [UxmlElement]
    public partial class EquipmentCardInventory : VisualElement
    {
        private VisualElement _equipment;
        private VisualElement _equipmentButton;
        private ListView _listView;
        public ListView ListView { get => _listView; }
        public EquipmentCardInventory() => Initialize();
        private async void Initialize()
        {
            AsyncOperationHandle<VisualTreeAsset> handle = Addressables.LoadAssetAsync<VisualTreeAsset>("UXML/EquipmentInventory.uxml");
            await handle.Task;

            if (handle.Status == AsyncOperationStatus.Succeeded && handle.Result != null)
            {
                var treeAsset = handle.Result;
                var container = treeAsset.Instantiate();
                container.style.width = Length.Percent(100);
                container.style.height = Length.Percent(100);
                container.RegisterCallback<KeyDownEvent>(e => e.StopImmediatePropagation());
                container.pickingMode = PickingMode.Ignore;
                hierarchy.Add(container);
                _equipment = container.Q<VisualElement>("EquipsInventory");
                _equipmentButton = container.Q<VisualElement>("EquipmentTextBox");
                _listView = container.Q<ListView>("EquipmentCardList");
                Debug.Log(ListView is null);
                _equipment.AddToClassList("equipment-inventory_close");
                _equipmentButton.RegisterCallback<ClickEvent>(x =>
                {
                    if (!_equipment.ClassListContains("equipment-inventory_close")) return;
                    _equipment.RemoveFromClassList("equipment-inventory_close");
                    _equipment.AddToClassList("equipment-inventory_open");
                });
                _equipment.RegisterCallback<MouseLeaveEvent>(x =>
                {
                    if (!_equipment.ClassListContains("equipment-inventory_open")) return;
                    _equipment.RemoveFromClassList("equipment-inventory_open");
                    _equipment.AddToClassList("equipment-inventory_close");
                });
                Debug.Log("ウィンドウは正常にロード完了");
            }
            else
            {
                Debug.LogError("Failed to load UXML file from Addressables: UXML/EquipmentInventory.uxml");
            }

            Addressables.Release(handle);
        }
    }
}
