using DCFrameWork.Enemy;
using DCFrameWork.MainSystem;
using System.Collections;
using UnityEngine;

namespace DCFrameWork
{
    public class TargetObjectManager : MonoBehaviour
    {
        [SerializeField] float _coolTime = 3f;

        [SerializeField] float _durabilityValue = 1000f;

        [SerializeField] float _currentValue = 1000f;

        TargetHealthBarManager _healthBarManager;

        private void Start()
        {
            _healthBarManager = GameObject.Find("HealthBarForTarget").GetComponent<TargetHealthBarManager>();
            _currentValue = _durabilityValue;
           
        }

        private void Update()
        {
            _healthBarManager.FollowTarget(transform);
        }
        private void OnTriggerEnter(Collider other)
        {

            if (other.gameObject.TryGetComponent(out BossEnemyManager boss) || _currentValue < 0)
            {
                // GameOverˆ—

            }

            if (other.gameObject.TryGetComponent(out IEnemy enemy))
            {
                _currentValue -= enemy.Plunder;
                _healthBarManager.BarFillUpdate(_currentValue / _durabilityValue);
                StartCoroutine(AttackTime(enemy));
    
            }

        }

        IEnumerator AttackTime(IEnemy enemy)
        {
            yield return FrameWork.PausableWaitForSecond(0.5f);
            enemy.DeathAction?.Invoke();
            enemy.DeathAction = null;    
        }

       

    }
}
