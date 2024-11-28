using System;
using System.Linq;
using UnityEngine;

namespace DCFrameWork.Enemy
{
    public class WaveManager : MonoBehaviour
    {
        [SerializeField] EnemyGenerator _enemyGenerators;
        [SerializeField] PhaseData _phaseData;

        int _waveCount;
        int _waveEnemySum;
        static int _deathEemyCount;

        public event Action _waveStartAction;
        public event Action _waveEndAction;
        public event Action _phaseEndAction;

        public int WaveCount { get => _waveCount; }
        /// <summary>
        /// ウェーブの進行状況を正規化した値
        /// </summary>
        public float WaveProgressNormal { get => (float)_deathEemyCount / (_waveEnemySum != 0 ? _waveEnemySum : 1); }
        WaveData NowWave { get => _phaseData._waveData[_waveCount]; }

        private void Awake()
        {
            _waveStartAction += () => NextWave();
            _waveStartAction?.Invoke();
        }


        void Update()
        {
            if (WaveProgressNormal == 1)
            {
                if (_waveCount != _phaseData._waveData.Length)
                {
                    _waveEndAction?.Invoke();
                    _waveStartAction?.Invoke();
                }
                else
                {
                    _phaseEndAction?.Invoke();
                }
            }
        }

        public void NextWave()
        {
            _deathEemyCount = 0;
            _enemyGenerators._waveData = NowWave;
            _waveEnemySum = NowWave._spawnData.Sum(data => data._enemyCount);
            _waveCount++;
        }
        public static void EnemyDeathCount() => ++_deathEemyCount;　//エネミー死亡時に呼んでほしい
    }
}
