using UnityEngine;

namespace DCFrameWork.Enemy
{
    [CreateAssetMenu(menuName = "GameData/PhaseData", fileName = "PhaseData")]
    public class PhaseData : ScriptableObject
    {
        [SerializeField] WaveDataSelection[] _waveData;
        public WaveDataSelection[] WaveData { get => _waveData; }
    }
    [System.Serializable]
    public class WaveDataSelection
    {
        [SerializeField] WaveData[] _selectionWaveData;
        public WaveData[] SelectintWaveData { get => _selectionWaveData; }
    }
    [System.Serializable]
    public struct WaveData
    {
        [SerializeField] EnemySpawnData[] _spawnData;
        public EnemySpawnData[] SpawnData { get => _spawnData; }
    }
    [System.Serializable]
    public struct EnemySpawnData
    {
        public EnemyKind _enemyType;
        public int _enemyLevel;
        public int _enemyCount;
        public float _spawnStartTime;
        public float _spawnEndTime;
        public int _spawnPoint;
    }
}