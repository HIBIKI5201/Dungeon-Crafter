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


        public Action _gameOverEvent;
        public Action<int> _getGold;

        [SerializeField] LevelManager _levelManager;
        [SerializeField] DropTableData _dropTable;
        [SerializeField] int _levelUpGachaCount = 3;
        private void Initialize()
        {
            _levelManager = GetComponentInChildren<LevelManager>();
            _levelManager.OnLevelChanged += x => GetRandomDefenseObj(); 
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
        public void ChangeDropTable(DropTableData dropTable) => _dropTable = dropTable;

        private void GetRandomDefenseObj()
        {
            var collection = _dropTable.GetRandomDefenseObj(_levelUpGachaCount);
            foreach (var item in collection)
            {
                CollectionSystem.Instans.SetDefenseObj(item);
                SetDefenseObject(item);
            }
        }

        [ContextMenu("GetRandomObj")]
        public void TestRandomObj()
        {
            var collection = _dropTable.GetRandomDefenseObj(_levelUpGachaCount);
            foreach (var item in collection)
            {
                Debug.Log(item);
                CollectionSystem.Instans.SetDefenseObj(item);
                SetDefenseObject(item);
            }
            var dObj=CollectionSystem.Instans.GetDefenseObjCollection();
            foreach (var item in dObj)
            {
                if (item != null)
                    Debug.Log(item.Value._name);
                else Debug.Log("Null");
            }
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
