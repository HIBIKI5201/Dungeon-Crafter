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
        protected EnemyData_B EnemyData { get => _data; }

        #region 共通ステータス
        private float _maxHealth;
        float IFightable.MaxHealth { get => _maxHealth; set => _maxHealth = value; }
        private float _currentHealth;
        float IFightable.CurrentHealth { get => _currentHealth; set { _currentHealth = value; HealthBarUpdate(); } }
        #endregion

        [SerializeField]
        protected float _levelRequirePoint;

        private EnemyHealthBarManager _healthBarManager;

        private Dictionary<ConditionType, int> _conditionList = new();
        Dictionary<ConditionType, int> IConditionable.ConditionList
        {
            get => _conditionList;
            set => _conditionList = value;
        }
        public int CountCondition(ConditionType type) => (_conditionList.TryGetValue(type, out int count)) ? count : 0;

        private Action _deathAction;
        Action IFightable.DeathAction { get => _deathAction; set => _deathAction = value; }

        private NavMeshAgent _agent;
        public void StartByPool(EnemyHealthBarManager enemyHealthBarManager, Vector3 targetPos)
        {
            _healthBarManager = enemyHealthBarManager;
            if (_data is null)
                Debug.Log("データがありません");
            LoadCommonData();
            _agent = GetComponent<NavMeshAgent>();
            GameBaseSystem.mainSystem?.AddPausableObject(this);
            HealthBarUpdate();
            Start_S();
            Initialize(targetPos);
        }

        /// <summary>
        /// サブクラスでのStartメソッド
        /// </summary>
        protected virtual void Start_S() { }

        private void OnEnable()
        {
            GameBaseSystem.mainSystem?.RemovePausableObject(this);
        }

        void IEnemy.Initialize(Vector3 targetPos) => Initialize(targetPos);
        void IEnemy.StartByPool(EnemyHealthBarManager enemyHealthBarManager, Vector3 targetPos) => StartByPool(enemyHealthBarManager, targetPos);

        /// <summary>
        /// 外部からの初期化処理
        /// ステータスの初期化などを行う
        /// </summary>
        private void Initialize(Vector3 targetPos)
        {
            _agent.speed = EnemyData.Dexterity;
            _currentHealth = _maxHealth;
            HealthBarUpdate();
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
            _maxHealth = data.MaxHealth;
            _currentHealth = data.CurrentHealth;

            LoadSpecificnData(data);
        }


        /// <summary>
        /// 設定した型パラメータに対応した専用変数を代入してください
        /// </summary>
        /// <param name="data">型パラメータのデータ</param>
        protected virtual void LoadSpecificnData(Data data) { }

        protected virtual void DeathBehaviour()
        {
            _deathAction?.Invoke();
            _deathAction = null;
        }

        /// <summary>
        /// NavMesh上のポジションへ移動する
        /// </summary>
        /// <param name="targetPos">移動目標の座標</param>
        protected void GoToTargetPos(Vector3 targetPos)
        {
            if (_agent.pathStatus != NavMeshPathStatus.PathInvalid)
            {
                _agent.SetDestination(targetPos);
            }
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
        void Initialize(Vector3 targetPos);
        void StartByPool(EnemyHealthBarManager enemyHealthBarManager, Vector3 targetPos);

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
            if (CurrentHealth <= 0)
            {
                return false;
            }

            CurrentHealth -= damage;
            HealthBarUpdate();

            if (CurrentHealth <= 0)
            {
                DeathAction?.Invoke();
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
    }

    public interface IConditionable
    {
        Dictionary<ConditionType, int> ConditionList { get; protected set; }

        void AddCondition(ConditionType type)
        {
            ConditionList[type] = ConditionList.TryGetValue(type, out var count) ? count + 1 : 1;
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
            }
        }
    }
}