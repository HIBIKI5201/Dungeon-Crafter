using DCFrameWork.MainSystem;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.AI;

namespace DCFrameWork.Enemy
{
    [RequireComponent(typeof(NavMeshAgent),typeof(Rigidbody))]
    
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

       
         private Transform _target;

        private bool _isDead;
        bool IFightable.IsDead { get => _isDead; set => _isDead = value; }

        public void StartByPool(EnemyHealthBarManager enemyHealthBarManager)
        {
            _healthBarManager = enemyHealthBarManager;
            if (_data is null)
            Debug.Log("�f�[�^������܂���");
            LoadCommonData();
            _target = GameObject.Find("TargetPos").GetComponent<Transform>();
            _agent = GetComponent<NavMeshAgent>();
            GoToTargetPos(_target.position);
            GameBaseSystem.mainSystem?.AddPausableObject(this);
            HealthBarUpdate();
            Start_S();
            Initialize();
        }

        /// <summary>
        /// �T�u�N���X�ł�Start���\�b�h
        /// </summary>
        protected virtual void Start_S() { }

        private void OnEnable()
        {
            GameBaseSystem.mainSystem?.RemovePausableObject(this);            
        }

        void IEnemy.Initialize()=> Initialize();
        void IEnemy.StartByPool(EnemyHealthBarManager enemyHealthBarManager)=> StartByPool(enemyHealthBarManager);   

        /// <summary>
        /// �O������̏���������
        /// �X�e�[�^�X�̏������Ȃǂ��s��
        /// </summary>
        private void Initialize()
        {
            _currentHealth = _maxHealth;
            _isDead = false;
            HealthBarUpdate();
            GoToTargetPos(_target.position);
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
        void Initialize();
        void StartByPool(EnemyHealthBarManager enemyHealthBarManager);
        
    }

    public interface IFightable
    {
        Action DeathAction { get; set; }

        void HealthBarUpdate();
        float MaxHealth { get; protected set; }
        float CurrentHealth { get; protected set; }

        bool IsDead { get; protected set; }

        /// <summary>
        /// �_���[�W���󂯂�
        /// </summary>
        /// <param name="damage">�_���[�W��</param>
        void HitDamage(float damage)
        {
            CurrentHealth -= damage;
            HealthBarUpdate();

            if (CurrentHealth <= 0 && IsDead == false)
            {
                DeathAction?.Invoke();
                IsDead = true;
            }
        }

        /// <summary>
        /// �񕜂��󂯂�
        /// </summary>
        /// <param name="amount">�񕜗�</param>
        void HitHeal(float heal)
        {
            CurrentHealth = Mathf.Min(CurrentHealth + heal, MaxHealth);
            HealthBarUpdate();
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