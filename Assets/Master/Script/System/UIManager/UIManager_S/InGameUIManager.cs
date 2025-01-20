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
        [SerializeField] PlayerManager _playerManager;
        [SerializeField] LevelManager _levelManager;
        [SerializeField] VisualTreeAsset _card;
        EquipmentCardInventory _equipmentList;
        BasicInformation _basicInformation;
        EquipmentSettingUI _equipmentSettingUI;
        //マウスが上にのっているかのブールを返すEvent
        public event Action<bool> OnMouseOnUI;

        //カードリストから削除するためのイベント
        public event Action<InventoryData> OnCardClick;
        protected override async Task LoadDocumentElement(VisualElement root)
        {
            _equipmentList = root.Q<EquipmentCardInventory>("EquipmentCardInventory");
            _basicInformation = root.Q<BasicInformation>("BasicInformation");
            _equipmentSettingUI = root.Q<EquipmentSettingUI>("EquipmentSettingUI");
            await _equipmentList.InitializeTask;
            await _basicInformation.InitializeTask;
            await _equipmentSettingUI.InitializeTask;
            _equipmentList.CardSet = _card;
            _stageManager.OnActivateTurretSelectedUI += EquipmentSettingUIUpdate;
            _phaseManager.PhaseProgressChanged += PhaseUpdate;
            _phaseManager._phaseEndAction += PhaseCount;
            _playerManager.OnGetGold += x => _basicInformation.Money = x;
            _levelManager.OnExperianceGained += _basicInformation.EXPGuageUpdate;
            _levelManager.OnExperianceGained += x => _basicInformation.Exp = x;
            _levelManager.OnLevelChanged += x => _basicInformation.Level = x;
            //マウスカーソルがUIの上に乗ったときのイベントの登録
            _equipmentList.OnMouseCursor += x => OnMouseOnUI?.Invoke(x);
            _basicInformation.OnMouseCursor += x => OnMouseOnUI?.Invoke(x);
            //インベントリが選択された時のイベント
            _equipmentList.OnInventory += () => _equipmentList.Inventoryset = _playerManager.TurretInventory;
            //カードが押された時のイベント
            _equipmentList.OnCardClick += x => _stageManager.SetTurret(x);
            _equipmentList.OnCardDest += x =>_playerManager.UseDefenseObject(x);
            OnMouseOnUI += x =>Debug.Log("変更" + x);
        }

        void EquipmentSettingUIUpdate(ITurret turret,bool turretbool)
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