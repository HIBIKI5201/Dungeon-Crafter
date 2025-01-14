using DCFrameWork.MainSystem;
using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DCFrameWork.Enemy
{
    public class WaveManager : MonoBehaviour
    {
        [SerializeField] EnemyGenerator _enemyGenerators;
        [SerializeField] PhaseData _phaseData;
        [SerializeField] float _startWaveWaitingTime = 20;

        int _waveCount;
        int _loopCount = 1;
        int _waveEnemySum;
        static int _deathEemyCount;

        public event Action _waveStartAction;
        public event Action _waveEndAction;
        public event Action _phaseEndAction;
        static Action _addDeathCountAction;

        public int WaveCount { get => _waveCount; private set => _waveCount = value; }
        int CurrentWaveIndex { get => _waveCount % _phaseData.WaveData.Length; }
        /// <summary>
        /// ウェーブの進行状況を正規化した値
        /// </summary>
        public float WaveProgressNormalize { get => (float)_deathEemyCount / (_waveEnemySum != 0 ? _waveEnemySum : 1); }

        public async void Initialize()
        {
            if (!_phaseData)
            {
                Debug.Log("PhaseData is null");
                return;
            }
            _addDeathCountAction = WaveEndCheck;
            _waveStartAction += () => NextWave();
            await Awaitable.WaitForSecondsAsync(_startWaveWaitingTime);
            _waveStartAction?.Invoke();
            Debug.Log("WaveStart");
        }

        void WaveEndCheck()
        {
            if (WaveProgressNormalize >= 1)
            {
                WaveCount++;
                if (CurrentWaveIndex == 0)
                {
                    Debug.Log("pheseEnd" + $"WaveCount:{WaveCount}");
                    _loopCount++;
                    _phaseEndAction?.Invoke();
                    _waveStartAction?.Invoke();
                }
                else
                {
                    Debug.Log("WaveEnd");
                    _waveEndAction?.Invoke();
                    _waveStartAction?.Invoke();
                }
            }
        }

        void NextWave()
        {
            _deathEemyCount = 0;

            //選択肢の中からランダムなデータを取得
            var waveData = _phaseData.WaveData[CurrentWaveIndex].
                SelectintWaveData[Random.Range(0,_phaseData.WaveData.Length)];

            waveData.SpawnData.Select(x => x._enemyLevel += _loopCount - 1);//周回ごとのレベル上昇
            Debug.Log(_loopCount - 1);
            _waveEnemySum = waveData.SpawnData.Sum(data => data._enemyCount);

            _enemyGenerators.Waving(waveData);
        }
        public static void EnemyDeathCount()　//エネミー死亡時に呼んでほしい
        {
            ++_deathEemyCount; 
            _addDeathCountAction?.Invoke();
        }

    }
}
