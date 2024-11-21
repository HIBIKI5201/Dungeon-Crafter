using System;
using TMPro;
using UnityEngine;

namespace DCFrameWork.Enemy
{
    [Serializable, CreateAssetMenu(menuName = "GameData/EnemyData/Base_Data", fileName = "EnemyData")]
    public class EnemyData_B : ScriptableObject
    {
        public float MaxHealth { get => _maxHealth; }
        [SerializeField]
        private float _maxHealth;

        public float Defense { get => _defense; }
        [SerializeField]
        private float _defense;

        public float Dexterity { get => _dexterity; }
        [SerializeField]
        private float _dexterity;

        public float SpecialChance { get => _specialChance; }
        [SerializeField]
        private float _specialChance;

        public float Plunder { get => _plunder; }
        [SerializeField]
        private float _plunder;

        public float DropEXP { get => _dropEXP; }
        [SerializeField]
        private float _dropEXP;

        public float DropGold { get => _dropGold; }
        [SerializeField]
        private float _dropGold;
    }
}