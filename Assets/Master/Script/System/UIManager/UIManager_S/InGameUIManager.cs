using System;
using System.Threading.Tasks;
using DCFrameWork.DefenseEquipment;
using DCFrameWork.Enemy;
using UnityEngine;
using UnityEngine.UIElements;

namespace DCFrameWork.UI
{
    public class InGameUIManager : UIManager_B
    {
        [SerializeField] StageManager _stageManager;
        [SerializeField] PhaseManager _phaseManager;
        EquipmentCardInventory _equipmentList;
        BasicInformation _basicInformation;
        EquipmentSettingUI _equipmentSettingUI;
        public event Action<bool> OnMouseOnUI;
        protected override async Task LoadDocumentElement(VisualElement root)
        {
            _equipmentList = root.Q<EquipmentCardInventory>("EquipmentCardInventory");
            _basicInformation = root.Q<BasicInformation>("BasicInformation");
            _equipmentSettingUI = root.Q<EquipmentSettingUI>("EquipmentSettingUI");
            await _equipmentList.InitializeTask;
            await _basicInformation.InitializeTask;
            await _equipmentSettingUI.InitializeTask;
            _stageManager.OnActivateTurretSelectedUI += EquipmentSettingUIUpdate;
            _phaseManager.PhaseProgressChanged += PhaseUpdate;
            _phaseManager._phaseEndAction += PhaseCount;
        }
        void EquipmentSettingUIUpdate(ITurret turret)
        {
            _equipmentSettingUI.EquipmentSettingWindowVisible = true;
        }
        void PhaseUpdate(float parsent)
        {
            _basicInformation.GuageMesh.UpdateGuage(parsent * 100);
        }
        void PhaseCount()
        {
            _basicInformation.PhaseText = _phaseManager.PhaseCount.ToString();
            _basicInformation.GuageMesh.UpdateGuage(0);
        }
    }
}