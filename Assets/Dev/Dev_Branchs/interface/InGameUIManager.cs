using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace DCFrameWork
{
    public class InGameUIManager : UIManager_B
    {
        [SerializeField] EquipmentCard[] card;
        EquipmentCardInventory _equipmentList;
        protected override async void LoadDocumentElement(VisualElement root)
        {
            _equipmentList = root.Q<EquipmentCardInventory>("EquipmentInventory");
            await _equipmentList.InitializeTask;
            _equipmentList.ListView.itemsSource = card;
        }
    }
}
