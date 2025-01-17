using UnityEngine;

namespace DCFrameWork.Enemy
{
    [CreateAssetMenu(menuName = "GameData/EnemyStateData", fileName = "EnemyStateData")]
    public class EnemyStateData : ScriptableObject
    {
        [SerializeField] private EnemyKindData[] _enemyKindData;
        public EnemyKindData[] EnemyKindData => _enemyKindData;
    }

    [System.Serializable]
    public class EnemyKindData
    {
        [SerializeField] private EnemyKind _enemyKind; // エネミーの種類
        public EnemyKind Kind => _enemyKind;

        [SerializeField] private EnemyLevelData[] _levelData; // レベルごとのステータス配列
        public EnemyLevelData[] LevelData => _levelData;
    }

    [System.Serializable]
    public class EnemyLevelData
    {
        [SerializeField] private int _level; // レベル
        public int Level => _level;

        [SerializeField] private EnemyState _state; // レベルに応じたエネミーのステータス
        public EnemyState State => _state;
    }

    [System.Serializable]
    public class EnemyState
    {
        [SerializeField] private float _maxHealth = 1f;
        public float MaxHealth => _maxHealth;

        [SerializeField] private float _plunder = 1f;
        public float Plunder => _plunder;

        [SerializeField] private float _dexterity = 1f;
        public float Dexterity => _dexterity;

        [SerializeField] private float _defense = 1f;
        public float Defense => _defense;

        [SerializeField] private float _dropEXP = 1f;
        public float DropEXP => _dropEXP;

        [SerializeField] private float _dropGold = 1f;
        public float DropGold => _dropGold;

        public override string ToString()
        {
            return $"sokudo{_dexterity} hp{_maxHealth}";
        }
    }

    public enum EnemyKind
    {
        Normal = 0,
        Defense = 1,
        Buff = 2,
        Fly = 3
    }
}
