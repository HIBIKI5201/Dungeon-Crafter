using System.Collections.Generic;
using UnityEngine;

namespace DCFrameWork.Enemy
{
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

        private List<ConditionType> _conditionList = new();

        private void Start()
        {
            _currentHealth = _maxHealth;
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
        }

        private void DeathBehivour()
        {
            Destroy(gameObject);
        }

        public void AddCondition(ConditionType type)
        {
            if (!_conditionList.Contains(type))
                _conditionList.Add(type);
        }

        public void RemoveCondition(ConditionType type)
        {
            if (_conditionList.Contains(type))
                _conditionList.Remove(type);
        }

        /// <summary>
        /// NavMesh上のポジションへ移動する
        /// </summary>
        /// <param name="pos">移動目標の座標</param>
        protected void GoToTargetPos(Vector3 pos)
        {

        }
    }
    public enum ConditionType
    {
        slow,
        weakness,
    }
}