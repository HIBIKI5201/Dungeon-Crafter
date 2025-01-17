using DCFrameWork.Enemy;
using DCFrameWork.MainSystem;
using System.Collections;
using UnityEngine;

namespace DCFrameWork
{
    public class TargetObjectManager : MonoBehaviour
    {
        [SerializeField] float _coolTime = 3f;

        [SerializeField] float _maxValue = 0f;

        TargetHealthBarManager _healthBarManager;

        PlayerManager _playerManager;

        private void Start()
        {
            _healthBarManager = GameObject.Find("HealthBarForTarget").GetComponent<TargetHealthBarManager>();
            _playerManager = FindAnyObjectByType<PlayerManager>();
            _maxValue = _playerManager.TreasureHp;
            
        }

        private void Update()
        {
            _healthBarManager.FollowTarget(transform);
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out IEnemy enemy))
            {
                StartCoroutine(AttackTime(enemy));
            }

        }

        IEnumerator AttackTime(IEnemy enemy)
        {
            yield return FrameWork.PausableWaitForSecond(0.5f);
            _playerManager.HPDown((int)enemy.Plunder);
            _healthBarManager.BarFillUpdate(_playerManager.TreasureHp / _maxValue);
            enemy.DeathAction?.Invoke();
            enemy.DeathAction = null;    
        }

       

    }
}
