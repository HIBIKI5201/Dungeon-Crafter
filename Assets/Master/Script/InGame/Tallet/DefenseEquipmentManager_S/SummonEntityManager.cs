using DCFrameWork.Enemy;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using DCFrameWork.MainSystem;
using DCFrameWork.DefenseEquipment;
using System.Collections;

namespace DCFrameWork
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class SummonEntityManager : MonoBehaviour, IPausable
    {
        bool _isPaused;
        [SerializeField] float _attackRate = 2;
        float _timer;
        [SerializeField] float _attackValue = 1;
        NavMeshAgent _agent;
        SummonTurretManager _turretManager;
        bool _isAttacked = false;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
        }
        void Start()
        {
            _timer = Time.time;
            if (transform.parent.TryGetComponent(out _turretManager))
                SetDestination();
        }

        void Update()
        {
            if (!_isPaused)
            {
                if (Time.time > _timer + _attackRate)
                {
                    if (_turretManager._enemyList.Count != 0)
                    {
                        _timer = Time.time;
                        _isAttacked = true;
                    }
                }
            }
        }

        public void SetDestination()
        {
            if (_turretManager._enemyList.Count != 0)
            {
                _agent.SetDestination(TargetSelect()[0].gameObject.transform.position);
            }
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (_isAttacked)
            {
                TargetsAddDamage(_turretManager._enemyList, _turretManager._attack);
                var enemy = _turretManager._enemyList;
                TargetAddCondition(enemy, ConditionType.weakness);
                if (enemy[0].TryGetComponent(out IConditionable conditionable))
                    StartCoroutine(TargetRemoveCondition(conditionable));
                _isAttacked = false;
            }
        }

        IEnumerator TargetRemoveCondition(IConditionable conditionable)
        {
            yield return FrameWork.PausableWaitForSecond(3f);
            conditionable.RemoveCondition(ConditionType.weakness);
        }
        public List<GameObject> TargetSelect()
        {
            return _turretManager._enemyList.OrderBy(x => Vector3.Distance(transform.position, x.transform.position)).Take(1).ToList();
        }

        void TargetsAddDamage(List<GameObject> enemies, float damage)
        {
            foreach (var enemy in enemies)
            {
                if (enemy.TryGetComponent(out IFightable component))
                    component.HitDamage(damage);
            }
        }

        void TargetAddCondition(List<GameObject> enemies, ConditionType type)
        {
            foreach (var enemy in enemies)
            {
                if (enemy.TryGetComponent(out IConditionable component))
                    component.AddCondition(type);
            }
        }
        public void Pause()
        {
            _isPaused = true;
        }

        public void Resume()
        {
            _isPaused = false;
        }
    }
}
