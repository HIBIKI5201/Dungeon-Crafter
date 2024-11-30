using System;
using System.Collections.Generic;
using UnityEngine;

namespace DCFrameWork
{
    public class PlayerManager : MonoBehaviour
    {
        int _treasureHp = 100;
        int _gold;
        LevelManager _levelManager;
        Dictionary<DefenseObjectsKind, int> _defenseObjectsValue;
        public Action _gameOverEvent;
        public Action<int> _getGold;

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
        public void GetDefenseObject(DefenseObjectsKind kind) => _defenseObjectsValue[kind]++;
        public void UseDefenseObject(DefenseObjectsKind kind) => _defenseObjectsValue[kind]--;
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
