using System;
using UnityEngine;
namespace DCFrameWork.DefenseEquipment
{
    [Serializable, CreateAssetMenu(menuName = "GameData/DefenseData/Common_Data", fileName = "DefebseEquipmentData")]
    public class DefenseEquipmentData_B : ScriptableObject
    {
        public float Attack { get => _attack; }
        [SerializeField]
        private float _attack = 1;

        public float Rate { get => _rate; }
        [SerializeField]
        private float _rate = 1;

        public float Critical { get => _critical; }
        [SerializeField]
        private float _critical = 1;

        public float Range { get => _range; }
        [SerializeField]
        private float _range = 1;

        public int LevelRequirePoint { get => _levelRequirePoint; }
        [SerializeField]
        private int _levelRequirePoint = 1;

        public float UseGold { get => _useGold; }
        [SerializeField]
        private float _useGold = 0;
    }
}