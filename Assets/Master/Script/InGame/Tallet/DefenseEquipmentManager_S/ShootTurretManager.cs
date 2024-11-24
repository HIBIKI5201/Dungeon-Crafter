using DCFrameWork.Enemy;
using UnityEngine;

namespace DCFrameWork.DefenseEquipment
{
    public class ShootTurretManager : DEShooterManager_SB<DefenseEquipmentData_B>
    {
        private float _range;

        float _timer = 0;
        bool _isPaused = false;

        protected override void Start_S()
        {
            _timer = Time.time;
        }
        protected override void Think() //UpDate ‚Æ“¯‹`
        {
            if (_isPaused)
                _timer += Time.deltaTime;

            if (Time.time > 1 / DefenseEquipmentData.Rate + _timer && _enemyList.Count > 0)
            {
                Attack();
                _timer = Time.time;
            }
        }
        protected override void LoadSpecificData(DefenseEquipmentData_B data)
        {
            _range = data.Range;
            RangeSet(_range);
        }
        protected override void Attack()
        {
            var criticalPoint = Random.Range(0, 100);
            var targetSelect = TargetSelect();
            TargetsAddDamage(targetSelect.Interface, criticalPoint <= DefenseEquipmentData.Critical ? DefenseEquipmentData.Attack * 3 : DefenseEquipmentData.Attack);
            TurretRotate(targetSelect.Obj);
        }

        void RangeSet(float range)
        {
            var coll = GetComponent<SphereCollider>();
            coll.radius = range;
            var syli = transform.GetChild(0);
            var size = new Vector3(coll.radius * 2, syli.transform.localScale.y, coll.radius * 2);
            syli.transform.localScale = size;

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
}