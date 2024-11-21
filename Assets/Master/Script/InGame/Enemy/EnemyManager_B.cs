using DCFrameWork.MainSystem;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace DCFrameWork.Enemy
{
    [RequireComponent(typeof(NavMeshAgent), typeof(Rigidbody))]

    public abstract class EnemyManager_B<Data> : MonoBehaviour, IEnemy, IPausable where Data : EnemyData_B
    {
        [SerializeField]
        private EnemyData_B _data;
        private Data _enemyData;
        protected Data EnemyData { get => _enemyData; }

        private float _maxHealth;
        float IFightable.MaxHealth { get => _maxHealth; set => _maxHealth = value; }
        private float _currentHealth;
        float IFightable.CurrentHealth { get => _currentHealth; set { _currentHealth = value; HealthBarUpdate(); } }

        [SerializeField]
        protected float _levelRequirePoint;

        private EnemyHealthBarManager _healthBarManager;

        private Dictionary<ConditionType, int> _conditionList = new();
        Dictionary<ConditionType, int> IConditionable.ConditionList { get => _conditionList; set => _conditionList = value; }
        public int CountCondition(ConditionType type) => (_conditionList.TryGetValue(type, out int count)) ? count : 0;

        private Action _deathAction;
        Action IFightable.DeathAction { get => _deathAction; set => _deathAction = value; }

        private NavMeshAgent _agent;
        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
        }

        void IEnemy.StartByPool(EnemyHealthBarManager enemyHealthBarManager, Vector3 targetPos)
        {
            GameBaseSystem.mainSystem?.AddPausableObject(this);
            _healthBarManager = enemyHealthBarManager;
            if (_data is null)
                Debug.Log("データがありません");
            LoadCommonData();
            enemyHealthBarManager.Initialize();
            HealthBarUpdate();
            Start_S();
        }

        /// <summary>
        /// サブクラスでのStartメソッド
        /// </summary>
        protected virtual void Start_S() { }

        private void Update()
        {
            _healthBarManager.FollowTarget(transform);
        }

        private void OnEnable()
        {
            GameBaseSystem.mainSystem?.RemovePausableObject(this);
        }

        /// <summary>
        /// 外部からの初期化処理
        /// ステータスの初期化などを行う
        /// </summary>
        void IEnemy.Initialize(Vector3 spawnPos, Vector3 targetPos, Action deathAction) => Initialize(spawnPos, targetPos, deathAction);
        private void Initialize(Vector3 spawnPos, Vector3 targetPos, Action deathAction)
        {
            _currentHealth = _maxHealth;
            ChangeSpeed(EnemyData.Dexterity);
            _deathAction = deathAction;
            HealthBarUpdate();

            gameObject.SetActive(true);
            _healthBarManager.gameObject.SetActive(true);
            gameObject.transform.position = spawnPos;
            _healthBarManager.FollowTarget(transform);
            GoToTargetPos(targetPos);
            Initialize_S();
        }

        /// <summary>
        /// サブクラス内の初期化処理
        /// </summary>
        protected virtual void Initialize_S() { }

        private void LoadCommonData()
        {
            Data data = _data as Data;
            _enemyData = data;
            _maxHealth = data.MaxHealth;

            LoadSpecificnData(data);
        }


        /// <summary>
        /// 設定した型パラメータに対応した専用変数を代入してください
        /// </summary>
        /// <param name="data">型パラメータのデータ</param>
        protected virtual void LoadSpecificnData(Data data) { }

        void IFightable.DeathBehaviour() => DeathBehaviour();
        protected virtual void DeathBehaviour()
        {
            gameObject.SetActive(false);
            _healthBarManager.gameObject.SetActive(false);
        }

        void IEnemy.Destroy()
        {
            Destroy(gameObject);
            Destroy(_healthBarManager.gameObject);
        }

        void IConditionable.ChangeCondition(ConditionType type)
        {
            switch (type)
            {
                case ConditionType.slow:
                    float newSpeed = EnemyData.Dexterity * (1 - (CountCondition(ConditionType.slow) * 0.5f));
                    ChangeSpeed(newSpeed);
                    break;
            }
        }

        /// <summary>
        /// NavMesh上のポジションへ移動する
        /// </summary>
        /// <param name="targetPos">移動目標の座標</param>
        protected void GoToTargetPos(Vector3 targetPos)
        {
            _agent.SetDestination(targetPos);
        }

        public void HealthBarUpdate()
        {
            _healthBarManager?.BarFillUpdate(_currentHealth / _maxHealth);
        }


        /// <summary>
        /// スピードを変える
        /// </summary>
        /// <param name="speed">スピード</param>
        private void ChangeSpeed(float speed)
        {
            _agent.speed = speed;
        }

        #region ポーズ処理
        void IPausable.Pause() => Pause();
        void IPausable.Resume() => Resume();
        protected abstract void Pause();
        protected abstract void Resume();
        #endregion
    }
    public enum ConditionType
    {
        slow,
        weakness,
    }

    public interface IEnemy : IFightable, IConditionable
    {
        void Initialize(Vector3 spawnPos, Vector3 targetPos, Action deathAction);
        void StartByPool(EnemyHealthBarManager enemyHealthBarManager, Vector3 targetPos);
        void Destroy();
    }

    public interface IFightable
    {
        Action DeathAction { get; set; }

        void HealthBarUpdate();
        float MaxHealth { get; protected set; }
        float CurrentHealth { get; protected set; }


        /// <summary>
        /// ダメージを受ける
        /// </summary>
        /// <param name="damage">ダメージ量</param>
        bool HitDamage(float damage)
        {
            CurrentHealth -= damage;
            HealthBarUpdate();

            if (CurrentHealth <= 0)
            {
                DeathAction?.Invoke();
                DeathAction = null;
                return false;
            }
            return true;
        }

        /// <summary>
        /// 回復を受ける
        /// </summary>
        /// <param name="amount">回復量</param>
        bool  HitHeal(float heal)
        {
            if (CurrentHealth == MaxHealth)
            {
                return false ;
            }
            CurrentHealth = Mathf.Min(CurrentHealth + heal, MaxHealth);
            HealthBarUpdate();
            return true;
        }

        void DeathBehaviour();
    }

    public interface IConditionable
    {
        Dictionary<ConditionType, int> ConditionList { get; protected set; }

        void AddCondition(ConditionType type)
        {
            ConditionList[type] = ConditionList.TryGetValue(type, out var count) ? count + 1 : 1;
            ChangeCondition(type);
        }

        void RemoveCondition(ConditionType type)
        {
            if (ConditionList.TryGetValue(type, out var count))
            {
                if (count > 1)
                {
                    ConditionList[type] = count - 1;
                }
                else
                {
                    ConditionList.Remove(type);
                }
                ChangeCondition(type);
            }
        }

        void ChangeCondition(ConditionType type);
    }
}