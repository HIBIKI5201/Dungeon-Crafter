using System;
using UnityEngine;

namespace DCFrameWork
{
    [Serializable, CreateAssetMenu(menuName = "GameData/DefenseData/Trap_Data", fileName = "DefebseEquipmentData")]

    public class TrapData : SummonData
    {
        public int BombRng { get => _bombRng; }
        [SerializeField]
        private int _bombRng = 1;
    }
}
