using System;
using System.Collections.Generic;
using UnityEngine;

namespace DCFrameWork
{
    public class PlayerManager : MonoBehaviour
    {
        int _treasureHp = 100;
        int _gold;
        Dictionary<DefenseObjectsKind, int> _defenseObjectsValue = new();

        LevelManager _levelManager;

        public Action _gameOverEvent;
        public Action<int> _getGold;

        [SerializeField] List<DefenseObjectsKind> _testData = new();
        private void Awake()
        {
            _levelManager = GetComponentInChildren<LevelManager>();
            _levelManager?.Initialize();
        }

        public int TreasureHp { get => _treasureHp; }

        public void HPDown(int damage)
        {
            _treasureHp -= damage;
            if (TreasureHp <= 0)
            {
                _gameOverEvent?.Invoke();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gold">ëùå∏Ç≥ÇπÇΩÇ¢ó </param>
        public void ChangeGold(int gold)
        {
            _gold += gold;
            _getGold?.Invoke(_gold);
        }
        public void SetDefenseObject(DefenseObjectsKind kind)
        {
            if (_defenseObjectsValue.ContainsKey(kind)) _defenseObjectsValue[kind]++;
            else _defenseObjectsValue.Add(kind, 1);
        }
        public void UseDefenseObject(DefenseObjectsKind kind)
        {
            if (_defenseObjectsValue.ContainsKey(kind)) _defenseObjectsValue[kind]--;
            else Debug.LogWarning($"{nameof(kind)}ÇÕë∂ç›ÇµÇ‹ÇπÇÒ");
        }
    }
    public enum DefenseObjectsKind
    {
        AreaTurret,
        LongShootTurret,
        MiddleShootTurret,
        ShortShootTurret,
        ReinforcementTurret,
        SummonTurret,
        TrapTurret,
        WeeknessTurret,
        SlowTurret
    }

}
