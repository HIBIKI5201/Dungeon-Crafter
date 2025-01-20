using DCFrameWork.DefenseEquipment;
using DCFrameWork.MainSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DCFrameWork
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] int _levelUpGachaCount = 3;
        [SerializeField] int _startTurretCount;
        [SerializeField] List<DefenseEquipmentDataBase> _defenseObjectData;
        [SerializeField] int _treasureHp = 10;
        [SerializeField] float _gold;



        List<InventoryData> _defenseObjectsValue = new();
        public List<InventoryData> TurretInventory { get => _defenseObjectsValue; private set => _defenseObjectsValue = value; }
        public int TreasureHp { get => _treasureHp; }
        public float Gold { get => _gold; }

        public event Action OnGameOver;
        public event Action<IEnumerable<InventoryData>> OnLevelUp;
        public event Action<float> OnGetGold;
        public event Action<InventoryData> OnGetDefenseObject;
        public event Action<InventoryData> OnUseDefenseObject;
        public event Action<List<InventoryData>> OnGachaRandomObjects;

        LevelManager _levelManager;
        public LevelManager LavelManager { get => _levelManager; }

        private void Start()
        {
            _levelManager = GetComponentInChildren<LevelManager>();
            _levelManager.OnLevelChanged += x => GetRandomDefenseObj();
            OnGameOver += () => GameBaseSystem.mainSystem.LoadScene(SceneKind.Home);
            OnGameOver += () => Debug.Log("GameOver");
        }
        public void Initialize()
        {
            for (int i = 0; i < _startTurretCount; i++)
            {
                SetDefenseObject(DefenseObjectsKind.MiddleShootTurret, 1);
            }
        }

        public void HPDown(int damage)
        {
            _treasureHp -= damage;
            if (TreasureHp <= 0)
            {
                OnGameOver?.Invoke();
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
                OnGetGold?.Invoke(_gold);
                return true;
            }
            return false;
        }
        public void AddEXP(float exp) => _levelManager.AddExperiancePoint(exp);
        public void SetDefenseObject(DefenseObjectsKind kind, int level = 1)
        {
            var inventory = new InventoryData(_defenseObjectData.Find(x=>x.Kind==kind), level);
            if (inventory.DefenseEquipmentData is null)
            {
                Debug.Log("DefenseData is null");
                return;
            }
            _defenseObjectsValue.Add(inventory);
            OnGetDefenseObject?.Invoke(inventory);
        }
        public void UseDefenseObject(DefenseObjectsKind kind, int level = 1)
        {
            var inventory = new InventoryData(_defenseObjectData.FirstOrDefault(x => x.Kind == kind), level);
            if (inventory.DefenseEquipmentData is null)
            {
                Debug.Log("DefenseData is null");
                return;
            }
            TurretInventory.Remove(inventory);
            OnUseDefenseObject?.Invoke(inventory);
        }

        private IEnumerable<InventoryData> GetRandomDefenseObj()
        {
            var defenseCopy = new List<DefenseEquipmentDataBase>(_defenseObjectData);
            var outList = new List<InventoryData>();
            for (int i = 0; i < _levelUpGachaCount; i++)
            {
                var sum = defenseCopy.Sum(x => x.DropChance);
                if (sum <= 0)
                {
                    Debug.Log("DropChance Sum = 0");
                }
                var randomCount = Random.Range(0, sum);
                var currentWeight = 0;
                for (var j = 0; j < defenseCopy.Count; j++)
                {
                    currentWeight += defenseCopy[j].DropChance;
                    if (currentWeight > randomCount)
                    {
                        outList.Add(new InventoryData(defenseCopy[j]));
                        defenseCopy.RemoveAt(j);
                        break;
                    }
                }
            }
            OnGachaRandomObjects?.Invoke(outList);
            return outList;
        }
        [ContextMenu("TestGacha")]
        public void TestRandom()
        {
            var test = GetRandomDefenseObj();
            foreach (var item in test)
            {
                Debug.Log(item.DefenseEquipmentData.name);
            }
        }
    }
    public class InventoryData
    {
        public DefenseEquipmentDataBase DefenseEquipmentData;
        public int Level;
        public string Name { get=>DefenseEquipmentData?.Name;}
        public string Explanation { get =>DefenseEquipmentData?.Explanation;}
        public DefenseObjectsKind Kind { get => DefenseEquipmentData.Kind; }
        public GameObject Prefab { get=>DefenseEquipmentData.Prefab;}
        public List<DefenseEquipmentData_B> DataLevelList { get =>  DefenseEquipmentData.DataLevelList;}
        public List<int> PowerUpRequireItem { get => DefenseEquipmentData.PowerUpRequireItem; }
        public InventoryData(DefenseEquipmentDataBase dataBase, int level = 1)
        {
            DefenseEquipmentData = dataBase;
            Level = level;
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
