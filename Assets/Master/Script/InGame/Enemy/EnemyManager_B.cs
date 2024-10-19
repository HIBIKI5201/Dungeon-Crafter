using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace DCFrameWork.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class EnemyManager_B : MonoBehaviour
    {
        [SerializeField]
        protected float _maxHealth;
        protected float _currentHealth;

        [SerializeField]
        protected float _defense;

        [SerializeField]
        protected float _dexterity;

        [SerializeField]
        protected float _specialChance;

        [SerializeField]
        protected float _plunder;

        [SerializeField]
        protected float _dropEXP;

        [SerializeField]
        protected float _dropGold;

        [SerializeField]
        protected float _levelRequirePoint;

        private EnemyHealthBarManager _healthBarManager;

        private Dictionary<ConditionType, int> _conditionList = new();

        private NavMeshAgent _agent;
        private void Start()
        {
            _currentHealth = _maxHealth;
            _agent = GetComponent<NavMeshAgent>();

            Start_S();
        }

        /// <summary>
        /// サブクラスでのStartメソッド
        /// </summary>
        protected virtual void Start_S() { }

        /// <summary>
        /// ダメージを受ける
        /// </summary>
        /// <param name="damage">ダメージ量</param>
        public void HitDamage(float damage)
        {
            _currentHealth -= damage;
            HealthBarUpdate();
            if (_currentHealth <= 0)
            {
                DeathBehivour();
            }
        }

        /// <summary>
        /// 回復を受ける
        /// </summary>
        /// <param name="amount">回復量</param>
        public void HitHeal(float amount)
        {
            _currentHealth = Mathf.Min(_currentHealth + amount, _maxHealth);
            HealthBarUpdate();
        }

        private void DeathBehivour()
        {
            Destroy(gameObject);
        }

        public void AddCondition(ConditionType type)
        {
            if (_conditionList.TryGetValue(type, out var count))
            {
                _conditionList[type] = count + 1;
            }
            else
            {
                _conditionList.Add(type, 1);
            }
        }

        public void RemoveCondition(ConditionType type)
        {
            if (_conditionList.TryGetValue(type, out var count))
            {
                _conditionList[type] = Mathf.Max(0, count - 1);
            }
        }

        public int CountCondition(ConditionType type) => (_conditionList.TryGetValue(type, out int count)) ? count : 0;

        /// <summary>
        /// NavMesh上のポジションへ移動する
        /// </summary>
        /// <param name="pos">移動目標の座標</param>
        protected void GoToTargetPos(Vector3 pos)
        {

        }

        private void HealthBarUpdate()
        {
            _healthBarManager?.BarFillUpdate(_currentHealth / _maxHealth);
        }
    }
    public enum ConditionType
    {
        slow,
        weakness,
    }
}