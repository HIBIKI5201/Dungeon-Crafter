using UnityEngine;
using UnityEngine.UIElements;

namespace DCFrameWork
{
    public class InGameUIManager : UIManager_B
    {
        [SerializeField] EquipmentCard[] card;
        EquipmentCardInventory _equipmentList;
        ListView list;
        protected override void LoadDocumentElement(VisualElement root)
        {
            _equipmentList = root.Q<EquipmentCardInventory>("EquipmentInventory");
            Debug.Log(_equipmentList.ListView is null);
            Debug.Log(_equipmentList.ListView.itemsSource is null);
            _equipmentList.ListView.itemsSource = card;
            Debug.Log(list.itemsSource);
        }
    }
}
