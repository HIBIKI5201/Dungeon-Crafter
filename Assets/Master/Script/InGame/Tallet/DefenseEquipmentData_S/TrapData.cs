using System;
using UnityEngine;

namespace DCFrameWork
{
    [Serializable, CreateAssetMenu(menuName = "GameData/DefenseData/Trap_Data", fileName = "DefebseEquipmentData")]

    public class TrapData : SummonData
    {
        public int BombRng { get => _bombRng; }
        [SerializeField, Header("爆弾の大きさ")]
        private int _bombRng = 1;
    }
}
