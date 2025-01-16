using System;
using System.Threading.Tasks;
using DCFrameWork.DefenseEquipment;
using UnityEngine;
using UnityEngine.UIElements;

namespace DCFrameWork.UI
{
    public class InGameUIManager : UIManager_B
    {
        [SerializeField] StageManager _stageManager;
        EquipmentCardInventory _equipmentList;
        BasicInformation _basicInformation;
        EquipmentSettingUI _equipmentSettingUI;
        protected override async Task LoadDocumentElement(VisualElement root)
        {
            _equipmentList = root.Q<EquipmentCardInventory>("EquipmentCardInventory");
            _basicInformation = root.Q<BasicInformation>("BasicInformation");
            _equipmentSettingUI = root.Q<EquipmentSettingUI>("EquipmentSettingUI");
            await _equipmentList.InitializeTask;
            await _basicInformation.InitializeTask;
            await _equipmentSettingUI.InitializeTask;
        }
        void EquipmentSettingUIUpdate(ITurret turret)
        {
            
        }
    }
}