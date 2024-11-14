using System;
using UnityEngine;

namespace DCFrameWork.Enemy
{
    [Serializable, CreateAssetMenu(menuName = "GameData/EnemyData/Base_Data", fileName = "EnemyData")]
    public class EnemyData_B : ScriptableObject
    {
        public float MaxHealth;
        public float CurrentHealth;
        public float Defense;
        public float Dexterity;
        public float SpecialChance;
        public float Plunder;
        public float DropEXP;
        public float DropGold;
    }
}