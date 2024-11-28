using UnityEngine;

namespace DCFrameWork.Enemy
{
    [CreateAssetMenu(menuName = "GameData/PhaseData", fileName = "PhaseData")]
    public class PhaseData : ScriptableObject
    {
        public WaveData[] _waveData;
    }
    [System.Serializable]
    public struct WaveData
    {
        public EnemySpawnData[] _spawnData;
    }
    [System.Serializable]
    public struct EnemySpawnData
    {
        public GameObject _enemy;//enumÇ…ïœçXó\íË
        public int _enemyLevel;
        public int _enemyCount;
        public float _spawnStartTime;
        public float _spawnEndTime;
        public float _spawnCoolTime;
        public int _spawnPoint;
    }
}