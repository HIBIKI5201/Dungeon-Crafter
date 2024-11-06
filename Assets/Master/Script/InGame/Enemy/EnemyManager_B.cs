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
        EnemyData_B _data;

        #region ���ʃX�e�[�^�X
        private float _maxHealth;
        float IFightable.MaxHealth { get => _maxHealth; set => _maxHealth = value; }
        private float _currentHealth;
        float IFightable.CurrentHealth { get => _currentHealth; set { _currentHealth = value; HealthBarUpdate(); } }
        protected float _defense;
        protected float _dexterity;
        protected float _specialChance;
        protected float _plunder;
        protected float _dropEXP;
        protected float _dropGold;
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
        public void StartByPool(EnemyHealthBarManager enemyHealthBarManager, Vector3 transform)
        {
            _healthBarManager = enemyHealthBarManager;
            if (_data is null)
                Debug.Log("�f�[�^������܂���");
            LoadCommonData();
            _agent = GetComponent<NavMeshAgent>();
            GoToTargetPos(transform);
            GameBaseSystem.mainSystem?.AddPausableObject(this);
            HealthBarUpdate();
            Start_S();
            Initialize(transform);
        }

        /// <summary>
        /// �T�u�N���X�ł�Start���\�b�h
        /// </summary>
        protected virtual void Start_S() { }

        private void OnEnable()
        {
            GameBaseSystem.mainSystem?.RemovePausableObject(this);
        }

        void IEnemy.Initialize(Vector3 transform) => Initialize(transform);
        void IEnemy.StartByPool(EnemyHealthBarManager enemyHealthBarManager, Vector3 transform) => StartByPool(enemyHealthBarManager, transform);

        /// <summary>
        /// �O������̏���������
        /// �X�e�[�^�X�̏������Ȃǂ��s��
        /// </summary>
        private void Initialize(Vector3 pos)
        {
            _currentHealth = _maxHealth;
            HealthBarUpdate();
            GoToTargetPos(pos);
            Initialize_S();
        }

        /// <summary>
        /// �T�u�N���X���̏���������
        /// </summary>
        protected virtual void Initialize_S() { }

        private void LoadCommonData()
        {
            Data data = _data as Data;
            _maxHealth = data.MaxHealth;
            _currentHealth = data.CurrentHealth;
            _defense = data.Defense;
            _dexterity = data.Dexterity;
            _specialChance = data.SpecialChance;
            _plunder = data.Plunder;
            _dropEXP = data.DropEXP;
            _dropGold = data.DropGold;

            LoadSpecificnData(data);
        }


        /// <summary>
        /// �ݒ肵���^�p�����[�^�ɑΉ�������p�ϐ��������Ă�������
        /// </summary>
        /// <param name="data">�^�p�����[�^�̃f�[�^</param>
        protected virtual void LoadSpecificnData(Data data) { }

        protected virtual void DeathBehaviour()
        {
            _deathAction = null;
            _deathAction?.Invoke();
        }

        /// <summary>
        /// NavMesh��̃|�W�V�����ֈړ�����
        /// </summary>
        /// <param name="pos">�ړ��ڕW�̍��W</param>
        protected void GoToTargetPos(Vector3 pos)
        {
            if (_agent.pathStatus != NavMeshPathStatus.PathInvalid)
            {
                _agent.SetDestination(pos);
            }
        }

        public void HealthBarUpdate()
        {
            _healthBarManager?.BarFillUpdate(_currentHealth / _maxHealth);
        }


        /// <summary>
        /// �X�s�[�h��ς���
        /// </summary>
        /// <param name="speed">�X�s�[�h</param>
        private void ChangeSpeed(float speed)
        {
            _agent.speed = speed;
        }

        #region �|�[�Y����
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
        void Initialize(Vector3 pos);
        void StartByPool(EnemyHealthBarManager enemyHealthBarManager, Vector3 transform);

    }

    public interface IFightable
    {
        Action DeathAction { get; set; }

        void HealthBarUpdate();
        float MaxHealth { get; protected set; }
        float CurrentHealth { get; protected set; }


        /// <summary>
        /// �_���[�W���󂯂�
        /// </summary>
        /// <param name="damage">�_���[�W��</param>
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
        /// �񕜂��󂯂�
        /// </summary>
        /// <param name="amount">�񕜗�</param>
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