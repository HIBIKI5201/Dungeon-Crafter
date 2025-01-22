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
        Gacha _gacha;
        //マウスが上にのっているかのブールを返すEvent
        public event Action<bool> OnMouseOnUI;
        //カードリストから削除するためのイベント
        public event Action<InventoryData> OnCardClick;
        //タレット削除のイベント
        public event Action OnTurretDelete;
        protected override async Task LoadDocumentElement(VisualElement root)
        {
            _equipmentList = root.Q<EquipmentCardInventory>("EquipmentCardInventory");
            _basicInformation = root.Q<BasicInformation>("BasicInformation");
            _equipmentSettingUI = root.Q<EquipmentSettingUI>("EquipmentSettingUI");
            _gacha = root.Q<Gacha>("Gacha");
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
            _equipmentSettingUI.OnMouseEvent += x => OnMouseOnUI?.Invoke(x);
            _gacha.OnMouseCursor += x => OnMouseOnUI?.Invoke(x);
            //インベントリが選択された時のイベント
            _equipmentList.OnInventory += () => _equipmentList.Inventoryset = _playerManager.TurretInventory;
            //カードが押された時のイベント
            _equipmentList.OnCardClick += x => _stageManager.SetTurret(x);
            _equipmentList.OnCardDest += x => _playerManager.UseDefenseObject(x);
            OnMouseOnUI += x => Debug.Log("変更" + x);
            //ガチャのイベント
            _gacha.Card = _card;
            _playerManager.OnGachaRandomObjects += x => _gacha.GachaBake(x);
            _gacha.OnGachaClose += (x, y) => _playerManager.SetDefenseObject(x, y);
        }
        void EquipmentSettingUIUpdate(ITurret turret, bool turretbool)
        {
            if (turret.Level != 4)
            {
                Debug.Log("タレットをクリックしました");
                Debug.Log(turret.CurrentData.name);
                _equipmentSettingUI.EquipmentSettingWindowVisible = false;
                _equipmentSettingUI.EquipmentName = turret.CurrentData.Name;
                _equipmentSettingUI.LevelText = "Lv " + turret.Level.ToString() + " -> " + (turret.Level + 1).ToString();
                _equipmentSettingUI.PowerText = "力 " + turret.CurrentData.Attack.ToString() + " -> " + turret.NextData.Attack.ToString();
                _equipmentSettingUI.FastText = "速さ" + turret.CurrentData.Rate.ToString() + " -> " + turret.NextData.Rate.ToString();
                _equipmentSettingUI.RangeText = "範囲" + turret.CurrentData.Range.ToString() + " -> " + turret.NextData.Rate.ToString();
                _equipmentSettingUI.PowerUpButtonText = "強化" + turret.CurrentData.LevelRequirePoint;
                _equipmentSettingUI.RemovalButtonName = turretbool ? "撤去" : "撤去できません";
                _equipmentSettingUI.PowerUpButton = () =>
                {
                    turret.LevelUp();
                    _equipmentSettingUI.EquipmentSettingWindowVisible = true;
                };
                _equipmentSettingUI.RemovalButton = () =>
                {
                    if (turretbool)
                    {
                        OnTurretDelete?.Invoke();
                        _equipmentSettingUI.EquipmentSettingWindowVisible = true;
                    }
                };
            }
            else
            {
                
                _equipmentSettingUI.EquipmentSettingWindowVisible = false;
                _equipmentSettingUI.name = turret.CurrentData.Name;
                _equipmentSettingUI.LevelText = string.Empty;
                _equipmentSettingUI.PowerText = string.Empty;
                _equipmentSettingUI.FastText = string.Empty;
                _equipmentSettingUI.RangeText = string.Empty;
                _equipmentSettingUI.PowerUpButtonText = "強化できません";
                _equipmentSettingUI.RemovalButtonName = turretbool ? "撤去" : "撤去できません";
                _equipmentSettingUI.PowerUpButton = () =>
                {
                    _equipmentSettingUI.EquipmentSettingWindowVisible = true;
                };
                _equipmentSettingUI.RemovalButton = () =>
                {
                    if (turretbool)
                    {
                        OnTurretDelete?.Invoke();
                        _equipmentSettingUI.EquipmentSettingWindowVisible = true;
                    }
                };
            }
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