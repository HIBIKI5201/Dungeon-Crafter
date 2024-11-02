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
        public Task InitializeTask { get; private set; }

        private const string _windowClose = "equipment-inventory_close";
        private const string _windowOpen = "equipment-inventory_open";

        private VisualElement _equipment;
        private VisualElement _equipmentButton;
        private VisualElement _backGround;
        private ListView _listView;
        public ListView ListView { get => _listView; }
        public EquipmentCardInventory() => InitializeTask = Initialize();
        private async Task Initialize()
        {
            AsyncOperationHandle<VisualTreeAsset> handle = Addressables.LoadAssetAsync<VisualTreeAsset>("UXML/EquipmentInventory.uxml");
            await handle.Task;

            if (handle.Status == AsyncOperationStatus.Succeeded && handle.Result != null)
            {
                var treeAsset = handle.Result;
                var container = treeAsset.Instantiate();
                container.style.width = Length.Percent(100);
                container.style.height = Length.Percent(100);
                hierarchy.Add(container);
                _equipment = container.Q<VisualElement>("EquipsInventory");
                _equipmentButton = container.Q<VisualElement>("EquipmentTextBox");
                _listView = container.Q<ListView>("EquipmentListView");
                _backGround = container.Q<VisualElement>("Backgound");
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
