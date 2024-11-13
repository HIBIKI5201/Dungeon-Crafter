using System;
using UnityEngine;
namespace DCFrameWork.DefenseEquipment
{
    [Serializable, CreateAssetMenu(menuName = "GameData/DefenseData/Common_Data", fileName = "DefebseEquipmentData")]
    public class DefenseEquipmentData_B : ScriptableObject
    {
        public float Attack { get => _attack; }
        [SerializeField]
        private float _attack;

        public float Rate { get => _rate; }
        [SerializeField]
        private float _rate;

        public float Critical {  get => _critical; }
        [SerializeField]
        private float _critical;
    }
}