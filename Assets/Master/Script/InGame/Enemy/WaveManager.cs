using System.Linq;
using UnityEngine;

namespace DCFrameWork.Enemy
{
    public class WaveManager : MonoBehaviour
    {
        [SerializeField] EnemyGenerator[] _enemyGenerators;
        [SerializeField] WaveData[] _waveData;

        int _waveCount;
        int _waveEnemySum;
        int _deathEemyCount;
        bool _isWave;
        
        public int WaveCount { get => _waveCount; }
        public bool IsWave { get => _isWave; }
        /// <summary>
        /// �E�F�[�u�̐i�s�󋵂𐳋K�������l
        /// </summary>
        public float WaveProgressNormal { get => (float)_deathEemyCount / (_waveEnemySum != 0 ? _waveEnemySum : 1); }
        WaveData NowWave { get => _waveData[_waveCount]; }
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Awake()
        {
            //EnemyGenerator�ɏ�����ǉ�
        }

        // Update is called once per frame
        void Update()
        {
            if (WaveProgressNormal == 1)
            {
                Debug.Log("WaveEnd");
                _isWave = false;
            }
        }

        public void NextWave()
        {
            if (_waveCount != _waveData.Length)
            {
                _deathEemyCount = 0;
                _waveEnemySum = NowWave._spawnData.Sum(data => data._enemyCount);
                _isWave = true;
                _waveCount++;
                Debug.Log("�E�F�[�u�ȍ~");
            }
        }
        public void EnemyDeathCount() => ++_deathEemyCount;//�G�l�~�[���S���ɌĂ�łق���
    }
}
