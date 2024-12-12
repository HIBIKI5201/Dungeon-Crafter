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
            if(other.gameObject.TryGetComponent(out IFightable fightable))
            {
                StartCoroutine(HitDamage(fightable.Plunder));
                
            }
        }

        IEnumerator HitDamage(float damage)
        {
            if(_currentValue <= 0)
            {

            }
            
            while(_currentValue > 0)
            {             
                _currentValue -= damage;
                _healthBarManager.BarFillUpdate(_currentValue / _durabilityValue);
                yield return FrameWork.PausableWaitForSecond(_coolTime);
            }
            
        }

    }
}
