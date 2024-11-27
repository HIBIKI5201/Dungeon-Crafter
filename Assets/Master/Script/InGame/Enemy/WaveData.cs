using UnityEngine;

namespace DCFrameWork.Enemy
{
    [System.Serializable, CreateAssetMenu(menuName = "GameData/WaveData", fileName = "WaveData")]
    public class WaveData : ScriptableObject
    {
        public EnemySpawnData[] _spawnData;
    }
    [System.Serializable]
    public struct EnemySpawnData
    {
        public GameObject _enemy;
        public int _enemyCount;
        public float _spawnStartTime;
        public float _spawnCoolTime;
        public int _simultaneousSpawnCount;
    }
}