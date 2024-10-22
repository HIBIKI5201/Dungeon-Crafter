using DCFrameWork.MainSystem;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace DCFrameWork.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class EnemyManager_B<Data> : MonoBehaviour, IPausable where Data : EnemyData_B
    {
        [SerializeField]
        EnemyData_B _data;

        #region ���ʃX�e�[�^�X
        protected float _maxHealth;
        protected float _currentHealth;
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

        private readonly Dictionary<ConditionType, int> _conditionList = new();

        private NavMeshAgent _agent;
        private void Start()
        {
            if (_data is null)
                Debug.Log("�f�[�^������܂���");
            LoadCommonData();

            _currentHealth = _maxHealth;
            _agent = GetComponent<NavMeshAgent>();

            GameBaseSystem.mainSystem.AddPausableObject(this as IPausable);

            Init_S();
        }

        private void OnDestroy()
        {
            GameBaseSystem.mainSystem.RemovePausableObject(this as IPausable);
        }

        /// <summary>
        /// �T�u�N���X�ł�Start���\�b�h
        /// </summary>
        protected virtual void Init_S() { }

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
        /// ���ʃX�e�[�^�X�����Ȃ��ꍇ�͈�����_�����ċ󃁃\�b�h�ɂ��Ă�������
        /// </summary>
        protected virtual void LoadSpecificnData(Data data) { }

        /// <summary>
        /// �_���[�W���󂯂�
        /// </summary>
        /// <param name="damage">�_���[�W��</param>
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
        /// �񕜂��󂯂�
        /// </summary>
        /// <param name="amount">�񕜗�</param>
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
        /// NavMesh��̃|�W�V�����ֈړ�����
        /// </summary>
        /// <param name="pos">�ړ��ڕW�̍��W</param>
        protected void GoToTargetPos(Vector3 pos)
        {
            _agent.SetDestination(pos);
        }

        private void HealthBarUpdate()
        {
            _healthBarManager?.BarFillUpdate(_currentHealth / _maxHealth);
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
}