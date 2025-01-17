using DCFrameWork.MainSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DCFrameWork
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] DropTableData _dropTable;
        [SerializeField] int _levelUpGachaCount = 3;
        [SerializeField] int _startGold;
        [SerializeField] int _startTurretCount;
        [SerializeField] int _startTreasureHp = 100;
        int _treasureHp = 100;
        float _gold;
        Dictionary<DefenseObjectsKind, int> _defenseObjectsValue = new();
        public Dictionary<DefenseObjectsKind, int> TurretInventory { get => _defenseObjectsValue; }
        public int TreasureHp { get => _treasureHp; }
        public float Gold { get => _gold; }

        public event Action _gameOverEvent;
        public event Action<IEnumerable<DefenseObjectsKind>> _levelUpAction;
        public event Action<float> _getGold;
        public event Action<DefenseObjectsKind> OnGetDefenseObject;
        public event Action<DefenseObjectsKind> OnUseDefenseObject;

        LevelManager _levelManager;
        public LevelManager LavelManager { get => _levelManager; }

        public void Initialize()
        {
            _treasureHp = _startTreasureHp;
            _levelManager = GetComponentInChildren<LevelManager>();
            _levelManager.OnLevelChanged += x => GetRandomDefenseObj();
            for (int i = 0; i < _startTurretCount; i++)
            {
                SetDefenseObject(DefenseObjectsKind.MiddleShootTurret);
            }
            _gameOverEvent += () => SceneChanger.LoadScene(SceneKind.Home);
        }

        public void HPDown(int damage)
        {
            _treasureHp -= damage;
            if (TreasureHp <= 0)
            {
                Debug.Log("GameOver");
                _gameOverEvent?.Invoke();
            }
        }
        /// <summary>
        /// インゲームのタレット強化用Gold
        /// </summary>
        /// <param name="gold">値を所持Goldに加算します。</param>
        public bool ChangeGold(float gold)
        {
            if (_gold + gold >= 0)
            {
                _gold += gold;
                _getGold?.Invoke(_gold);
                return true;
            }
            return false;
        }
        public void AddEXP(float exp) => _levelManager.AddExperiancePoint(exp);
        public void SetDefenseObject(DefenseObjectsKind kind)
        {
            if (_defenseObjectsValue.ContainsKey(kind)) _defenseObjectsValue[kind]++;
            else _defenseObjectsValue.Add(kind, 1);
            OnGetDefenseObject?.Invoke(kind);
        }
        public void UseDefenseObject(DefenseObjectsKind kind)
        {
            if (_defenseObjectsValue.ContainsKey(kind)) _defenseObjectsValue[kind]--;
            else Debug.LogWarning($"{nameof(kind)}が存在しません");
            OnUseDefenseObject?.Invoke(kind);
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
            _levelUpAction?.Invoke(list);
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
