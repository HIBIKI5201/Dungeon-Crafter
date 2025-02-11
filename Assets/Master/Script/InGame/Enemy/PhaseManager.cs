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
        [SerializeField] float _startWaitingTimer = 10;
        [SerializeField] float _phaseWaitingTime = 5;

        [Space(10)]
        [SerializeField] int _battleBGMID = 5;
        [SerializeField] int _breakBGMID = 6;

        int _phaseCount;
        int _loopCount = 1;
        int _phaseEnemySum;
        int _deathEemyCount;

        public event Action _phaseStartAction;
        public event Action _phaseEndAction;

        /// <summary>
        /// 引数1：正規化されたPhaseの
        /// </summary>
        public event Action<float> PhaseProgressChanged;

        public int PhaseCount { get => _phaseCount; private set => _phaseCount = value; }
        int CurrentPhaseIndex { get => _phaseCount % _phaseData.PhaseData.Length; }
        /// <summary>
        /// ウェーブの進行状況を正規化した値
        /// </summary>
        public float PhaseProgressNormalize { get => (float)_deathEemyCount / (_phaseEnemySum != 0 ? _phaseEnemySum : 1); }

        public async void Initialize()
        {
            if (!_phaseData)
            {
                Debug.Log("PhaseData is null");
                return;
            }

            //イベント追加
            _phaseStartAction += () => GameBaseSystem.mainSystem.PlayBGM(_battleBGMID, BGMMode.CrossFade);
            _phaseEndAction += () => GameBaseSystem.mainSystem.PlayBGM(_breakBGMID, BGMMode.CrossFade);

            await FrameWork.PausableWaitForSecondAsync(_startWaitingTimer, destroyCancellationToken);
            NextPhase();
            _phaseStartAction?.Invoke();
            PhaseEndCheck();
        }

        async void PhaseEndCheck()
        {
            if (PhaseProgressNormalize >= 1)
            {
                if (CurrentPhaseIndex == 0 && PhaseCount != 0)
                {
                    Debug.Log("pheseEnd " + $"WaveCount:{PhaseCount}");
                    _loopCount++;
                }
                _phaseEndAction?.Invoke();
                await FrameWork.PausableWaitForSecondAsync(_phaseWaitingTime, destroyCancellationToken);
                NextPhase();
                _phaseStartAction?.Invoke();
            }
        }

        void NextPhase()
        {
            _deathEemyCount = 0;

            //選択肢の中からランダムなデータを取得
            var phaseData = _phaseData.PhaseData[CurrentPhaseIndex].
                SelectintPhaseData[0];//ランダムではなく1択になったため固定化、めちゃめちゃよくない実装

            phaseData.SpawnData.Select(x => x._enemyLevel += _loopCount - 1);//周回ごとのレベル上昇
            _phaseEnemySum = phaseData.SpawnData.Length;

            _enemyGenerators.Waving(phaseData);
            PhaseProgressChanged?.Invoke(PhaseProgressNormalize);
            PhaseCount++;//フェーズカウントを進める処理が先にあるとelement0が呼ばれないため移動
        }
        public void EnemyDeathCount()　//エネミー死亡時に呼んでほしい
        {
            ++_deathEemyCount;
            PhaseEndCheck();
            PhaseProgressChanged?.Invoke(PhaseProgressNormalize);
        }
    }
}
