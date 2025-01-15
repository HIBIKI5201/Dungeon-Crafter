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

        private void Start()
        {
            _healthBarManager = GameObject.Find("HealthBarForTarget").GetComponent<TargetHealthBarManager>();
            _maxValue = PlayerManager.TreasureHp;
           
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
            PlayerManager.HPDown((int)enemy.Plunder);
            _healthBarManager.BarFillUpdate(PlayerManager.TreasureHp / _maxValue);
            enemy.DeathAction?.Invoke();
            enemy.DeathAction = null;    
        }

       

    }
}
