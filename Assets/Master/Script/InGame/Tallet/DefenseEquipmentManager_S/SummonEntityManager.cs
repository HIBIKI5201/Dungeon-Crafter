using DCFrameWork.Enemy;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using DCFrameWork.MainSystem;

namespace DCFrameWork
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class SummonEntityManager : MonoBehaviour, IPausable
    {
        bool _isPaused;
        [SerializeField] float _attackRate = 2;
        float _timer;
        List<GameObject> _enemyList = new();
        [SerializeField] float _attackValue = 1;
        NavMeshAgent _agent;
        [SerializeField] EntityType _entityType;
        enum EntityType
        {
            attack,
            slow,
            attackandslow,
        }

        void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
        }

        void Update()
        {
            var attackTime = 1 / _attackRate * Time.deltaTime;
            if (!_isPaused)
            {
                _timer += attackTime;
                if (_timer > 1)
                {
                    if (_enemyList.Count != 0)
                    {
                        var targetSelect = TargetSelect();
                        _agent.destination = targetSelect[0].transform.position;
                        switch (_entityType)
                        {
                            case EntityType.attack:
                                TargetsAddDamage(targetSelect, _attackValue);
                                break;
                            case EntityType.slow:
                                TargetAddCondition(targetSelect, ConditionType.slow);
                                break;
                            case EntityType.attackandslow:
                                TargetsAddDamage(targetSelect, _attackValue);
                                TargetAddCondition(targetSelect, ConditionType.slow);
                                break;
                        }
                        _timer = 0;
                    }
                }
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (!_isPaused)
            {
                if (other.TryGetComponent<IFightable>(out _))
                {
                    Debug.Log(other.gameObject.name);
                    _enemyList.Add(other.gameObject);
                }
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (!_isPaused)
            {
                if (other.TryGetComponent<IFightable>(out _))
                {
                    _enemyList.Remove(other.gameObject);

                }
            }
        }
        List<GameObject> TargetSelect()
        {
            return _enemyList.OrderBy(x => Vector3.Distance(transform.position, x.transform.position)).Take(1).ToList();
        }

        void TargetsAddDamage(List<GameObject> enemies, float damage)
        {
            Debug.Log("Attack Now");
            foreach (var enemy in enemies)
            {
                if (enemy.TryGetComponent(out IFightable component))
                    component.HitDamage(damage);
            }
        }

        void TargetAddCondition(List<GameObject> enemies, ConditionType type)
        {
            Debug.Log("Add Condition");
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
