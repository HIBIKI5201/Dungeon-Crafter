using DCFrameWork.DefenseEquipment;
using DCFrameWork.Enemy;
using DCFrameWork.MainSystem;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DCFrameWork
{
    public class TrapManager : MonoBehaviour, IPausable
    {
        TrapTurretManager _turretManager;
        bool _isPaused = false;

        void Awake()
        {
            transform.parent.TryGetComponent(out _turretManager);

        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_isPaused)
            {
                if (other.TryGetComponent(out IFightable component) && !other.TryGetComponent(out FlyEnemyManager _))
                {
                    component.HitDamage(_turretManager.EntityAttack);
                    Destroy(gameObject);
                }
            }
        }
        private void OnDestroy()
        {
            _turretManager.ThisRemove(gameObject);
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
