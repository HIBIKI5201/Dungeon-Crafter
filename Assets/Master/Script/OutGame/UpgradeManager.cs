using DCFrameWork.DefenseEquipment;
using DCFrameWork.MainSystem;
using UnityEngine;

namespace DCFrameWork
{
    public class UpgradeManager : DefenseEquipmentManager_B<DefenseEquipmentData_B>
    {
        GameSaveData _gameSaveData;
        private void Start()
        {
            _gameSaveData = new GameSaveData();
        }
        public void BuyUpgrade(DefenseObjectsKind defenseObjectsKind)
        {
            if (_gameSaveData.PowerUpItemValue >= DefenseEquipmentData.LevelRequirePoint)
            {
                _gameSaveData.PowerUpItemValue -= DefenseEquipmentData.LevelRequirePoint;
                _gameSaveData.PowerUpDatas[(int)defenseObjectsKind]++;
            }
        }

        protected override void Think()
        {
            throw new System.NotImplementedException();
        }

        protected override void Pause()
        {
            throw new System.NotImplementedException();
        }

        protected override void Resume()
        {
            throw new System.NotImplementedException();
        }
    }
}
