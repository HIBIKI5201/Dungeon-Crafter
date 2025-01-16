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
        //Task
        public Task InitializeTask { get; private set; }
        //const
        private const string _windowClose = "equipment-inventory_close";
        private const string _windowOpen = "equipment-inventory_open";
        private const string _equipmentingButtonElementNotActive = "equipmenting-button-element-not-active";
        private const string _equipmentingButtonElementActive = "equipmenting-button-element-active";
        private const string _equipmentTextBoxNotActive = "equipment-text-box-not-active";
        private const string _equipmentTextBoxActive = "equipment-text-box-active";

        //private
        private VisualElement _equipment;
        private VisualElement _equipmentButton;
        private VisualElement _backGround;
        private VisualElement _doorButton;
        private VisualElement _wallButton;
        private VisualElement _doorIcon;
        private VisualElement _wallIcon;
        private VisualElement _equipmentingButtonElement;
        private VisualElement _cancel;
        private VisualElement _anotherEquipment;
        private Label _doorText;
        private Label _doorGold;
        private Label _wallGold;
        private Label _wallText;
        private ListView _listView;
       
        //public
        public ListView ListView { get => _listView; }
        public VisualElement DoorButton { get => _doorButton; }
        public VisualElement WallButton { get => _wallButton; }
        public Label DoorGold { get => _doorGold; }
        public Label WallGold { get => _wallGold; }
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
                _backGround = container.Q<VisualElement>("Background");
                _doorButton = container.Q<VisualElement>("DoorButton");
                _wallButton = container.Q<VisualElement>("WallButton");
                _doorIcon = container.Q<VisualElement>("DoorIcon");
                _wallIcon = container.Q<VisualElement>("WallIcon");
                _doorText = container.Q<Label>("DoorText");
                _doorGold = container.Q<Label>("DoorGold");
                _wallText = container.Q<Label>("WallText");
                _wallGold = container.Q<Label>("WallGold");
                _listView = container.Q<ListView>("EquipmentCardList");
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
        void Equipmenting()
        {

        }
        void UnEquipmenting()
        {

        }
    }
}
