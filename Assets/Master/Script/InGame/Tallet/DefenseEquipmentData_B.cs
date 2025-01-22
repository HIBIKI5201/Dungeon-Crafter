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

        public float LevelRequirePoint { get => _levelRequirePoint; }
        [SerializeField]
        private float _levelRequirePoint = 100;
        /// <summary>
        /// タレットの名前
        /// </summary>
        public string Name { get => _data.Name; }
        /// <summary>
        /// タレットの説明文
        /// </summary>
        public string Explanation { get => _data.Explanation; }
        /// <summary>
        /// ガチャの出現確率
        /// </summary>
        public int DropChance { get => _data.DropChance; }
        [SerializeField]
        private DefenseEquipmentDataBase _data;
    }
}