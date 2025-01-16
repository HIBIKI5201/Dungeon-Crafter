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
        [SerializeField] EnemyWaveData _phaseData;
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
        /// �E�F�[�u�̐i�s�󋵂𐳋K�������l
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

            //�I�����̒����烉���_���ȃf�[�^���擾
            var waveData = _phaseData.WaveData[CurrentWaveIndex].
                SelectintWaveData[Random.Range(0,_phaseData.WaveData.Length)];

            waveData.SpawnData.Select(x => x._enemyLevel += _loopCount - 1);//���񂲂Ƃ̃��x���㏸
            Debug.Log(_loopCount - 1);
            _waveEnemySum = waveData.SpawnData.Length;

            _enemyGenerators.Waving(waveData);
        }
        public static void EnemyDeathCount()�@//�G�l�~�[���S���ɌĂ�łق���
        {
            ++_deathEemyCount; 
            _addDeathCountAction?.Invoke();
        }

    }
}
