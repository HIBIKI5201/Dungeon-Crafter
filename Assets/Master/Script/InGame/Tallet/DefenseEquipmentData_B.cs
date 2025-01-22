using System;
using UnityEngine;
namespace DCFrameWork.DefenseEquipment
{
    [Serializable, CreateAssetMenu(menuName = "GameData/DefenseData/Common_Data", fileName = "DefebseEquipmentData")]
    public class DefenseEquipmentData_B : ScriptableObject
    {
        public float Attack { get => _attack; }
        [SerializeField,Header("攻撃力")]
        private float _attack = 1;

        public float Rate { get => _rate; }
        [SerializeField, Header("攻撃のレート")]
        private float _rate = 1;

        public float Critical { get => _critical; }
        [SerializeField, Header("クリティカルの確率％")]
        private float _critical = 1;

        public float Range { get => _range; }
        [SerializeField, Header("射程距離")]
        private float _range = 1;

        public float LevelRequirePoint { get => _levelRequirePoint; }
        [SerializeField, Header("レベルアップに必要なゴールド")]
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
        [SerializeField, Header("タレットのデータベース")]
        private DefenseEquipmentDataBase _data;
    }
}