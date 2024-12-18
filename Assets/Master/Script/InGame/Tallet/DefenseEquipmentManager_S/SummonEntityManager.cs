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
        NavMeshAgent _agent;
        SummonTurretManager _turretManager;
        bool _isAttacked = true;
        GameObject _target;
        [SerializeField] float _hitStopTime = 1;
        //Vector3 _startPos;
        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            if (transform.parent.TryGetComponent(out _turretManager))
                SetTarget();
            //_startPos = transform.position;
        }
        void Start()
        {
            _timer = 1 / _attackRate;
        }

        void Update()
        {
            if (!_isPaused)
            {
                if (!_isAttacked)
                {
                    _timer -= Time.deltaTime;
                    if (_timer <= 0)
                    {
                        _isAttacked = true;
                        _timer = 1 / _attackRate;
                    }
                }
                if (_turretManager.EnemyList.Count > 0)
                {
                    if (IsTargetSet())
                    {
                        SetTarget();
                    }
                    if (_target != null && _agent.isOnNavMesh)
                    {
                        _agent.SetDestination(_target.transform.position);
                    }
                }
                else
                {
                    //Debug.Log("タレットくんに帰る");
                    //if (_agent.isOnNavMesh)
                    //    _agent.SetDestination(_startPos);
                    if (_agent.isOnNavMesh)
                        _agent.ResetPath();
                    _target = null;
                }
            }
        }

        public void SetTarget()
        {
            if (_turretManager.EnemyList.Count != 0)
            {
                _target = TargetSelect().gameObject;
            }
            else
            {
                _target = null;
            }
        }
        public bool IsTargetSet()
        {
            return _target == null || !_turretManager.EnemyList.Contains(_target);
        }
        private void OnTriggerEnter(Collider other)
        {
            if (_isAttacked && _target)
            {
                if (other.gameObject == _target.gameObject)
                {
                    Debug.Log("あたった");
                    TargetsAddDamage(_target, _turretManager.EntityAttack);
                    TargetAddCondition(_target, ConditionType.weakness);
                    TargetAddHitStop(_target, _hitStopTime);
                    if (_target.TryGetComponent(out IConditionable conditionable))
                        StartCoroutine(TargetRemoveCondition(conditionable));
                    _isAttacked = false;
                }
            }
        }
        IEnumerator TargetRemoveCondition(IConditionable conditionable)
        {
            yield return FrameWork.PausableWaitForSecond(3f);
            conditionable.RemoveCondition(ConditionType.weakness);
        }
        public GameObject TargetSelect()
        {
            return _turretManager.EnemyList.OrderBy(x => Vector3.Distance(transform.position, x.transform.position)).First();
        }

        void TargetsAddDamage(GameObject enemy, float damage)
        {
            if (enemy.TryGetComponent(out IFightable component))
                if (!component.HitDamage(damage))
                {
                    _turretManager.EnemyList.Remove(enemy);
                }
        }

        void TargetAddCondition(GameObject enemy, ConditionType type)
        {
            if (enemy.TryGetComponent(out IConditionable component))
                component.AddCondition(type);
        }
        void TargetAddHitStop(GameObject enemy, float time)
        {
            if (enemy.TryGetComponent(out IEnemy component))
            {
                component.StopEnemy(time);
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