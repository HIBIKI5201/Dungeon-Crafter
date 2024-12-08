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
        bool _isAttacked = false;
        GameObject _target;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            if (transform.parent.TryGetComponent(out _turretManager))
                SetDestination();
        }
        void Start()
        {
            _timer = Time.time;
        }

        void Update()
        {
            if (_isAttacked)
            {
                _timer += Time.deltaTime;
            }
            if (!_isPaused)
            {
                if (Time.time > _timer + _attackRate && !_isAttacked)
                {
                    _isAttacked = true;
                }
                if (IsTargetSet())
                {
                    _agent.SetDestination(_turretManager.transform.position);
                }
            }
        }

        public void SetDestination()
        {
            if (_turretManager._enemyList.Count != 0 && _agent.isOnNavMesh)
            {
                _target = TargetSelect().gameObject;
                _agent.SetDestination(_target.transform.position);
            }
        }
        public bool IsTargetSet()
        {
            return _agent.destination == Vector3.zero;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (_isAttacked)
            {
                if (other.gameObject == _target.gameObject)
                {
                    Debug.Log("あたった");
                    TargetsAddDamage(_target, _turretManager._attack);
                    TargetAddCondition(_target, ConditionType.weakness);
                    TargetAddHitStop(_target);
                    if (_target.TryGetComponent(out IConditionable conditionable))
                        StartCoroutine(TargetRemoveCondition(conditionable));
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
            return _turretManager._enemyList.OrderBy(x => Vector3.Distance(transform.position, x.transform.position)).First();
        }

        void TargetsAddDamage(GameObject enemy, float damage)
        {
            if (enemy.TryGetComponent(out IFightable component))
                component.HitDamage(damage);
            _isAttacked = false;
        }

        void TargetAddCondition(GameObject enemy, ConditionType type)
        {
            if (enemy.TryGetComponent(out IConditionable component))
                component.AddCondition(type);
        }
        void TargetAddHitStop(GameObject enemy)
        {
            if (enemy.TryGetComponent(out IEnemy component))
            {
                //HitStop処理をここに
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