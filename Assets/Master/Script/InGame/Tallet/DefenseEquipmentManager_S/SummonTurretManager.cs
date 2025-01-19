using DCFrameWork.Enemy;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DCFrameWork.DefenseEquipment
{
    public class SummonTurretManager : DEEntityManager_SB<SummonData>
    {
        float _timer = 0;
        bool _isPaused = false;
        public float EntityAttack { get => Attack; }
        [NonSerialized] public List<GameObject> EnemyList = new();
        Vector3 _position;
        protected override void Start_S()
        {
            _timer = Time.time;
        }
        protected override void Think() //UpDate
        {
            if (_isPaused)
                _timer += Time.deltaTime;

            if (Time.time > 1 / Rate + _timer)
            {
                _timer = Time.time;
                var summonPos = SummonPosition();
                _position = summonPos;
                int count = 0;
                bool isChecked = false;
                while (!Check(summonPos))
                {
                    summonPos = SummonPosition();
                    _position = summonPos;
                    count++;
                    if (count <= 10)
                    {
                        isChecked = true;
                        break;
                    }
                }
                if (!isChecked)
                    Summon(summonPos, DefenseEquipmentData.MaxCount);
            }
        }
        protected override void Pause()
        {
            _isPaused = true;
        }

        protected override void Resume()
        {
            _isPaused = false;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (!_isPaused)
            {
                if (other.TryGetComponent<IFightable>(out _) && !other.TryGetComponent<FlyEnemyManager>(out _))
                {
                    EnemyList.Add(other.gameObject);
                }
                if (_entityList.Count > 0)
                {
                    List<SummonEntityManager> summonList = new();
                    _entityList.ForEach(x => summonList.Add(x.GetComponent<SummonEntityManager>()));
                    summonList.Where(x => x.IsTargetSet()).ToList().ForEach(x => x.SetTarget());
                }
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (!_isPaused)
            {
                if (other.TryGetComponent<IFightable>(out _))
                {
                    EnemyList.Remove(other.gameObject);
                }
            }
        }

        protected void TargetsAddDamage(List<GameObject> enemies, float damage)
        {
            foreach (var enemy in enemies)
            {
                if (enemy.TryGetComponent(out IFightable component))
                    component.HitDamage(damage);
            }
        }

        protected void TargetAddCondition(List<GameObject> enemies, ConditionType type)
        {
            foreach (var enemy in enemies)
            {
                if (enemy.TryGetComponent(out IConditionable component))
                    component.AddCondition(type);
            }
        }
#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            Gizmos.color = Check(_position) ? Color.red : Color.green;

            Vector3 boxCastOrigin = _position + new Vector3(0, 8, 0);
            Vector3 boxCastDirection = Vector3.down;
            float boxCastDistance = 18f;
            Quaternion boxCastRotation = Quaternion.identity;

            Gizmos.matrix = Matrix4x4.TRS(boxCastOrigin, boxCastRotation, _boxCastSize * 2);
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one);

            Vector3 boxCastEnd = boxCastOrigin + boxCastDirection.normalized * boxCastDistance;

            Gizmos.matrix = Matrix4x4.identity;
            Gizmos.DrawLine(boxCastOrigin, boxCastEnd);

            Gizmos.matrix = Matrix4x4.TRS(boxCastEnd, boxCastRotation, _boxCastSize * 2);
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
        }
#endif
    }
}