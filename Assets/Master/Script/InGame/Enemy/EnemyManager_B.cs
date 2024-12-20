using DCFrameWork.MainSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.InputManagerEntry;

namespace DCFrameWork.Enemy
{
    [RequireComponent(typeof(NavMeshAgent), typeof(Rigidbody))]

    public abstract class EnemyManager_B<Data, Data2> : MonoBehaviour, IEnemy, IPausable where Data : EnemyData_B where Data2 : EnemyRiseData
    {
        #region �f�[�^��
        [SerializeField]
        private Data _enemyData;

        private float _enemyLevel = 1;
        [SerializeField]
        private Data2 _enemyRiseData;
        float IFightable.MaxHealth { get => ChangeStates(_enemyRiseData.MaxHealth, _enemyLevel, _enemyData.MaxHealth); set { ChangeStates(_enemyRiseData.MaxHealth, _enemyLevel, _enemyData.MaxHealth); } }
        private float _currentHealth;
        float IFightable.CurrentHealth { get => _currentHealth; set { _currentHealth = value; HealthBarUpdate(); } }

        private float _defense;
        protected float Defense { get => _defense; set { _defense = value; } }
        protected float Dexterity { get => _agent.speed; set => _agent.speed = value; }

        float IFightable.Plunder { get => ChangeStates(_enemyRiseData.Plunder, _enemyLevel, _enemyData.Plunder); }

        float IFightable.DropEXP { get => ChangeStates(_enemyRiseData.DropEXP, _enemyLevel, _enemyData.DropEXP);}//set { ChangeStates(_enemyRiseData.DropEXP, _enemyLevel, _enemyData.DropEXP); } }

        float IFightable.DropGold { get => ChangeStates(_enemyRiseData.DropGold, _enemyLevel, _enemyData.DropGold); }// set { ChangeStates(_enemyRiseData.DropGold, _enemyLevel, _enemyData.DropGold); } }


        [SerializeField]
        protected float _levelRequirePoint;
        #endregion
        private EnemyHealthBarManager _healthBarManager;
        private NavMeshAgent _agent;

        #region �C���^�[�t�F�[�X
        private Dictionary<ConditionType, int> _conditionList = new();
        Dictionary<ConditionType, int> IConditionable.ConditionList { get => _conditionList; }
        public int CountCondition(ConditionType type) => (_conditionList.TryGetValue(type, out int count)) ? count : 0;

        private Action _deathAction;

        Vector3 IEnemy.position { get => this.transform.position; set => this.transform.position = value; }
        Action IFightable.DeathAction { get => _deathAction; set => _deathAction = value; }
        #endregion


        private const float _debuff = 0.2f;
        protected float Debuff { get => _debuff; }

        private const float _buff = 0.1f;
        protected float Buff { get => _buff; }
        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _currentHealth = _enemyData.MaxHealth;
        }

        void IEnemy.StartByPool(EnemyHealthBarManager enemyHealthBarManager)
        {
            GameBaseSystem.mainSystem?.AddPausableObject(this);
            _healthBarManager = enemyHealthBarManager;
            if (_enemyData is null)
                Debug.Log("�f�[�^������܂���");
            LoadCommonData();
            enemyHealthBarManager.Initialize();
            HealthBarUpdate();
            Start_S();
        }

        /// <summary>
        /// �T�u�N���X�ł�Start���\�b�h
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
        /// �O������̏���������
        /// �X�e�[�^�X�̏������Ȃǂ��s��
        /// </summary>
        void IEnemy.Initialize(Vector3 spawnPos, Vector3 targetPos, Action deathAction) => Initialize(spawnPos, targetPos, deathAction);
        private void Initialize(Vector3 spawnPos, Vector3 targetPos, Action deathAction)
        {
            _currentHealth = _enemyData.MaxHealth;
            HealthBarUpdate();
            ChangeSpeed(Dexterity);
            _deathAction = deathAction;
            gameObject.transform.position = spawnPos;
            gameObject.SetActive(true);
            _healthBarManager.gameObject.SetActive(true);
            _healthBarManager.FollowTarget(transform);
            GoToTargetPos(targetPos);
            Initialize_S();
        }

        /// <summary>
        /// �T�u�N���X���̏���������
        /// </summary>
        protected virtual void Initialize_S() { }

        private void LoadCommonData()
        {
            LoadSpecificnData(_enemyData);
        }


        /// <summary>
        /// �ݒ肵���^�p�����[�^�ɑΉ�������p�ϐ��������Ă�������
        /// </summary>
        /// <param name="data">�^�p�����[�^�̃f�[�^</param>
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
                    float newSpeed = _enemyData.Dexterity * (1 - (CountCondition(ConditionType.slow) * _debuff));
                    ChangeSpeed(newSpeed);
                    break;
                case ConditionType.weakness:
                    float newDefense = _enemyData.Defense * (1 - (CountCondition(ConditionType.weakness) * _debuff));
                    ChangeDefense(newDefense);
                    break;
                case ConditionType.defensive:
                    newDefense = _enemyData.Defense * (1 + (CountCondition(ConditionType.defensive) * _buff));
                    ChangeDefense(newDefense);
                    break;
            }

        }


        void IEnemy.GotoTargetPos(Vector3 targetPos) => GoToTargetPos(targetPos);
        /// <summary>
        /// NavMesh��̃|�W�V�����ֈړ�����
        /// </summary>
        /// <param name="targetPos">�ړ��ڕW�̍��W</param>
        public void GoToTargetPos(Vector3 targetPos)
        {
            _agent.SetDestination(targetPos);
        }

        void IFightable.HealthBarUpdate() => HealthBarUpdate();
        private void HealthBarUpdate()
        {
            _healthBarManager?.BarFillUpdate(_currentHealth / _enemyData.MaxHealth);
        }



        float IEnemy.ChangeStates(float rise, float level, float param) => ChangeStates(rise, level, param);
        private float ChangeStates(float rise, float level, float param)
        {
            float defaultParam = param;
            float riseParam = defaultParam + (level - 1) * rise;
            return riseParam;
        }


        /// <summary>
        /// �X�s�[�h��ς���
        /// </summary>
        /// <param name="speed">�X�s�[�h</param>
        private void ChangeSpeed(float speed)
        {
            _agent.speed = speed;
        }

        /// <summary>
        /// �h��͂�ς���
        /// </summary>
        /// <param name="defense">�h���</param>
        private void ChangeDefense(float defense)
        {
            _defense = defense;
        }

        void IEnemy.SetLevel(float level) => SetLevel(level);
        private void SetLevel(float level)
        {
            _enemyLevel = level;
            _defense = ChangeStates(_enemyRiseData.Defense, level, _enemyData.Defense);
            Dexterity = ChangeStates(_enemyRiseData.Dexterity, level, _enemyData.Dexterity);
            _currentHealth = ChangeStates(_enemyRiseData.MaxHealth, level, _enemyData.MaxHealth);
        }

        void IEnemy.StopEnemy(float time) => StopEnemy(time);
        private void StopEnemy(float time)
        {
            if(gameObject.activeInHierarchy)
            StartCoroutine(StopTime(time)); 
        }
        IEnumerator StopTime(float time)
        {
            float speed = _agent.speed;
            _agent.speed = 0;
            yield return FrameWork.PausableWaitForSecond(time);
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
        normal,
        slow,
        weakness,
        defensive,
    }

    public interface IEnemy : IFightable, IConditionable
    {
        void Initialize(Vector3 spawnPos, Vector3 targetPos, Action deathAction);
        void StartByPool(EnemyHealthBarManager enemyHealthBarManager);
        void Destroy();

        float ChangeStates(float rise, float level, float param);

        void SetLevel(float level);

        void StopEnemy(float time);

        void GotoTargetPos(Vector3 targetPos);

        Vector3 position { get; set; }
    }

    public interface IFightable
    {
        Action DeathAction { get; set; }

        protected void HealthBarUpdate();
        float MaxHealth { get; protected set; }
        float CurrentHealth { get; protected set; }

        float Plunder { get; }

        float DropGold {  get; }

        float DropEXP {  get; }

        //float EnemyLevel { get; protected set; }

        /// <summary>
        /// �_���[�W���󂯂�
        /// </summary>
        /// <param name="damage">�_���[�W��</param>
        bool HitDamage(float damage)
        {
            CurrentHealth -= damage;
            HealthBarUpdate();
            if (CurrentHealth <= 0)
            {
                DeathAction += DeathBehaviour;
                DeathAction?.Invoke();
                DeathAction = null;
                PlayerManager.ChangeGold(DropGold);
                PlayerManager.AddEXP(DropEXP);
                return false;
            }
            return true;
        }

        /// <summary>
        /// �񕜂��󂯂�
        /// </summary>
        /// <param name="amount">�񕜗�</param>
        bool HitHeal(float heal)
        {
            if (CurrentHealth == MaxHealth)
            {
                return false;
            }
            CurrentHealth = Mathf.Min(CurrentHealth + heal, MaxHealth);
            HealthBarUpdate();
            return true;
        }

        void DeathBehaviour();
    }

    public interface IConditionable
    {
        protected Dictionary<ConditionType, int> ConditionList { get; }

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