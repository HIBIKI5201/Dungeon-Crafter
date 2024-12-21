using System;
using UnityEngine;

namespace DCFrameWork.DefenseEquipment
{
    public class ReinforcementTurretManager : DefenseEquipmentManager_B<DefenseEquipmentData_B>
    {
        bool _isPaused = false;
        private event Action<ReinforceStatus> _reinforceEvent;
        protected override void Think() // UpDate ‚Æ“¯‹`
        {

        }
        protected override void LoadSpecificData(DefenseEquipmentData_B data)
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_isPaused)
            {
                if (other.TryGetComponent(out ITurret component) && !other.TryGetComponent(out ReinforcementTurretManager _))
                {
                    ReinforceStatus status = new(Attack, Rate, Range, Critical);
                    component.Reinforce(status);
                    _reinforceEvent += component.Reinforce;
                }
            }
        }

        private void OnDisable()
        {
            _reinforceEvent.Invoke(ReinforceStatus.Default);
        }


        protected override void Pause()
        {
            _isPaused = true;
        }

        protected override void Resume()
        {
            _isPaused = false;
        }

    }

    public struct ReinforceStatus
    {
        public float Attack;
        public float Rate;
        public float Range;
        public float Critical;

        public static ReinforceStatus Default { get => new ReinforceStatus(0, 0, 0, 0); }
        public ReinforceStatus(float attack, float rate, float range, float critical)
        {
            Attack = attack; Rate = rate; Range = range; Critical = critical;
        }
    }
}