using DCFrameWork.MainSystem;
using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DCFrameWork.Enemy
{
    public class PhaseManager : MonoBehaviour
    {
        [SerializeField] EnemyGenerator _enemyGenerators;
        [SerializeField] EnemyPhaseData _phaseData;
        [SerializeField] float _phaseWaitingTime = 20;

        int _phaseCount;
        int _loopCount = 1;
        int _phaseEnemySum;
        static int _deathEemyCount;

        public event Action _phaseStartAction;
        public event Action _phaseEndAction;
        public event Action<float> PhaseProgressChanged;

        public int WaveCount { get => _phaseCount; private set => _phaseCount = value; }
        int CurrentWaveIndex { get => _phaseCount % _phaseData.WaveData.Length; }
        /// <summary>
        /// �E�F�[�u�̐i�s�󋵂𐳋K�������l
        /// </summary>
        public float WaveProgressNormalize { get => (float)_deathEemyCount / (_phaseEnemySum != 0 ? _phaseEnemySum : 1); }

        public void Initialize()
        {
            if (!_phaseData)
            {
                Debug.Log("PhaseData is null");
                return;
            }
            _phaseStartAction += WaveEndCheck;
            _phaseStartAction += () => NextWave();
            _phaseStartAction?.Invoke();
        }

        async void WaveEndCheck()
        {
            if (WaveProgressNormalize >= 1)
            {
                WaveCount++;
                if (CurrentWaveIndex == 0 && WaveCount != 0)
                {
                    Debug.Log("pheseEnd" + $"WaveCount:{WaveCount}");
                    _loopCount++;
                }
                _phaseEndAction?.Invoke();
                await FrameWork.PausableWaitForSecondAsync(_phaseWaitingTime);
                _phaseStartAction?.Invoke();
            }
        }

        void NextWave()
        {
            _deathEemyCount = 0;

            //�I�����̒����烉���_���ȃf�[�^���擾
            var waveData = _phaseData.WaveData[CurrentWaveIndex].
                SelectintWaveData[Random.Range(0, _phaseData.WaveData.Length)];

            waveData.SpawnData.Select(x => x._enemyLevel += _loopCount - 1);//���񂲂Ƃ̃��x���㏸
            Debug.Log(_loopCount - 1);
            _phaseEnemySum = waveData.SpawnData.Length;

            _enemyGenerators.Waving(waveData);
        }
        public void EnemyDeathCount()�@//�G�l�~�[���S���ɌĂ�łق���
        {
            ++_deathEemyCount;
            PhaseProgressChanged?.Invoke(WaveProgressNormalize);
        }

    }
}
