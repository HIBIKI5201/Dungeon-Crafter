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
        private float _maxHealth = 0f;

        public float Plunder { get => _plunder; }
        [SerializeField]
        private float _plunder = 0f;

        public float Dexterity { get => _dexterity; }
        [SerializeField]
        private float _dexterity = 0f;

        public float Defense { get => _defense; }
        [SerializeField]
        private float _defense = 0f;


        public float DropEXP { get => _dropEXP; }
        [SerializeField]
        private float _dropEXP = 0f;


        public float DropGold { get => _dropGold; }
        [SerializeField]
        private float _dropGold = 0f;


    
    }
}