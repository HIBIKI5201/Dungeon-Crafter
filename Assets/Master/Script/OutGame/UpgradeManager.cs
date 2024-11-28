using DCFrameWork.DefenseEquipment;
using DCFrameWork.MainSystem;
using UnityEngine;
using System.Collections.Generic;
using System;

namespace DCFrameWork
{
    public class UpgradeManager : MonoBehaviour
    {
        GameSaveData _gameSaveData;
        List<DefenseEquipmentElement> list = new();
        [Serializable]
        struct DefenseEquipmentElement
        {
            public DefenseEquipmentDataBase Data;
            public DefenseObjectsKind Kind;
        }
        public void BuyUpgrade(DefenseObjectsKind defenseObjectsKind)
        {
            int price = list.Find(e => e.Kind == defenseObjectsKind).Data.PowerUpRequireItem[_gameSaveData.PowerUpDatas[(int)defenseObjectsKind]];
            if (_gameSaveData.PowerUpItemValue >= price)
            {
                _gameSaveData.PowerUpItemValue -= price;
                _gameSaveData.PowerUpDatas[(int)defenseObjectsKind]++;
            }
        }
    }
}
