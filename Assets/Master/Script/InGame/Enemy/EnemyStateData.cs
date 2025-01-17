
using UnityEngine;
namespace DCFrameWork.Enemy
{
    [CreateAssetMenu(menuName = "GameData/EnemyStateData", fileName = "EnemyStateData")]
    public class EnemyStateData : ScriptableObject
    {
        [SerializeField] SetEnemyKind[] _enemyKind;
        public SetEnemyKind[] EnemeyKind { get => _enemyKind; }
    }

    [System.Serializable]
    public struct SetEnemyKind
    {
        [SerializeField] EnemyState[] _state;

        public EnemyState[] State { get => _state; }
    }

    [System.Serializable]
    public class EnemyState
    {
        public float MaxHealth { get => _maxHealth; }
        [SerializeField]
        private float _maxHealth = 1f;

        public float Plunder { get => _plunder; }
        [SerializeField]
        private float _plunder = 1f;

        public float Dexterity { get => _dexterity; }
        [SerializeField]
        private float _dexterity = 1f;

        public float Defense { get => _defense; }
        [SerializeField]
        private float _defense = 1f;


        public float DropEXP { get => _dropEXP; }
        [SerializeField]
        private float _dropEXP = 1f;


        public float DropGold { get => _dropGold; }
        [SerializeField]
        private float _dropGold = 1f;


    }

}



