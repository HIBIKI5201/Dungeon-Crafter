using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace DCFrameWork
{
    public class InGameUIManager : UIManager_B
    {
        [SerializeField] private EquipmentCard[] card;
        private EquipmentCardInventory _equipmentList;

        protected override async void LoadDocumentElement(VisualElement root)
        {
            _equipmentList = root?.Q<EquipmentCardInventory>("EquipmentInventory");

            if (_equipmentList == null)
            {
                Debug.LogWarning("EquipmentInventory ‚ªŒ©‚Â‚©‚è‚Ü‚¹‚ñ‚Å‚µ‚½");
                return;
            }

            await _equipmentList.InitializeTask;

            if (_equipmentList.ListView != null)
            {
                _equipmentList.ListView.itemsSource = card;
            }
            else
            {
                Debug.LogWarning("ListView ‚ªŒ©‚Â‚©‚è‚Ü‚¹‚ñ‚Å‚µ‚½");
            }
        }
    }
}
