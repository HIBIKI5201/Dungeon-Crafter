using System;
using System.Collections.Generic;
using UnityEngine;

namespace DCFrameWork
{
    public class PlayerManager : MonoBehaviour
    {
        static int _treasureHp = 100;
        static float _gold;
        static Dictionary<DefenseObjectsKind, int> _defenseObjectsValue = new();
        public static Dictionary<DefenseObjectsKind, int> TurretInventory{  get => _defenseObjectsValue; }
        public static int TreasureHp { get => _treasureHp; }
        public static float Gold { get => _gold; }

        public static event Action _gameOverEvent;
        public static event Action<IEnumerable<DefenseObjectsKind>> _levelUpAction;
        public static event Action<float> _getGold;

        static LevelManager _levelManager;
        [SerializeField] DropTableData _dropTable;
        [SerializeField] int _levelUpGachaCount = 3;
        [SerializeField] int _startGold;
        [SerializeField] int _startTurretCount;

        public void Initialize()
        {
            _levelManager = GetComponentInChildren<LevelManager>();
            _levelManager.OnLevelChanged += x => GetRandomDefenseObj();
            for(int i = 0; i < _startTurretCount; i++)
            {
                SetDefenseObject(DefenseObjectsKind.MiddleShootTurret);
            }
            _startGold = 0;
        }

        public static void HPDown(int damage)
        {
            _treasureHp -= damage;
            if (TreasureHp <= 0)
            {
                _gameOverEvent?.Invoke();
            }
        }
        /// <summary>
        /// インゲームのタレット強化用Gold
        /// </summary>
        /// <param name="gold">値を所持Goldに加算します。</param>
        public static bool ChangeGold(float gold)
        {
            if (_gold + gold >= 0)
            {
                _gold += gold;
                _getGold?.Invoke(_gold);
                return true;
            }
            return false;
        }
        public static void AddEXP(float exp) => _levelManager.AddExperiancePoint(exp);
        public static void SetDefenseObject(DefenseObjectsKind kind)
        {
            if (_defenseObjectsValue.ContainsKey(kind)) _defenseObjectsValue[kind]++;
            else _defenseObjectsValue.Add(kind, 1);
        }
        public static void UseDefenseObject(DefenseObjectsKind kind)
        {
            if (_defenseObjectsValue.ContainsKey(kind)) _defenseObjectsValue[kind]--;
            else Debug.LogWarning($"{nameof(kind)}が存在しません");
        }
        public void ChangeDropTable(DropTableData dropTable) => _dropTable = dropTable;

        private IEnumerable<DefenseObjectsKind> GetRandomDefenseObj()
        {
            var collection = _dropTable.GetRandomDefenseObj(_levelUpGachaCount);
            var list = new List<DefenseObjectsKind>();
            foreach (var item in collection)
            {
                CollectionSystem.Instans.SetDefenseObj(item);
                list.Add(item);
            }
            _levelUpAction(list);
            return list;
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
            var dObj = CollectionSystem.Instans.GetDefenseObjCollection();
            foreach (var item in dObj)
            {
                if (item != null)
                    Debug.Log(item.Name);
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
