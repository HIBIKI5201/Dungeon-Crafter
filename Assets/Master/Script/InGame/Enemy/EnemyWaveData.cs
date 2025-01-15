using UnityEngine;

namespace DCFrameWork.Enemy
{
    [CreateAssetMenu(menuName = "GameData/EnemyWaveData", fileName = "EnemyWaveData")]
    public class EnemyWaveData : ScriptableObject
    {
        [SerializeField] RandomPhaseDataSelection[] _PhaseData;
        public RandomPhaseDataSelection[] WaveData { get => _PhaseData; }
    }
    [System.Serializable]
    public class RandomPhaseDataSelection
    {
        [SerializeField] PhaseData[] _selectionPhaseData;
        public PhaseData[] SelectintWaveData { get => _selectionPhaseData; }
    }
    [System.Serializable]
    public struct PhaseData
    {
        [SerializeField] EnemySpawnData[] _spawnData;
        public EnemySpawnData[] SpawnData { get => _spawnData; }
    }
    [System.Serializable]
    public class EnemySpawnData
    {
        public EnemyKind _enemyType;
        public int _enemyLevel = 1;
        public float _spawnTime = 0;
        [HideInInspector] public int _spawnPoint = 0;
    }
}