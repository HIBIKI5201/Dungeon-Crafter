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
            if (transform.parent.TryGetComponent(out _turretManager))
            {
                Debug.Log(_turretManager.gameObject.name);
            }
            else
            {
                Debug.Log("Ž¸”s");
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IFightable component))
            {
                component.HitDamage(_turretManager._attack);
                Destroy(gameObject);
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
