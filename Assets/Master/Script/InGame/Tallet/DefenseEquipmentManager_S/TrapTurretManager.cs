using System;
using Unity.VisualScripting;
using UnityEngine;
namespace DCFrameWork.DefenseEquipment
{
    public class TrapTurretManager : DEEntityManager_SB<TrapData>
    {

        float _timer = 0;
        bool _isPaused = false;
        bool _isCoolTimed = false;
        public float EntityAttack { get => Attack; }
        public int BombRange { get => DefenseEquipmentData.BombRng; }
        [SerializeField] AnimationCurve _animationCurve;
        Vector3 _position;

        protected override void Start_S()
        {
            _timer = Time.time;
        }

        protected override void Think() //UpDate �Ɠ��`
        {
            if (_isPaused || _isCoolTimed)
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
        override protected bool Check(Vector3 pos)
        {
            var size = BombRange * 5;
            Physics.BoxCast(pos + new Vector3(0, 8, 0), Vector3.one * size / 2, Vector3.down, out RaycastHit hit, Quaternion.identity, 18f);

            if (hit.collider != null)
            {
                return hit.collider.gameObject.layer == Mathf.Log(_groundLayer.value, 2);
            }
            return false;
        }
        [ContextMenu("StartCoolTime")]
        public void StartCoolTime()
        {
            _isCoolTimed = true;
            foreach (var entity in _entityList)
            {
                Destroy(entity);
            }
        }
        [ContextMenu("EndCoolTime")]
        public void EndCoolTime()
        {
            _isCoolTimed = false;
        }
        public void ThisRemove(GameObject trap)
        {
            _entityList.Remove(trap);
        }
        protected override void LoadSpecificData(TrapData data)
        {

        }
        protected override void Pause()
        {
            _isPaused = true;
        }

        protected override void Resume()
        {
            _isPaused = false;
        }
#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            Gizmos.color = Check(_position) ? Color.red : Color.green;

            // BoxCast �̃f�[�^���v�Z
            Vector3 boxCastOrigin = _position + new Vector3(0, 8, 0);
            Vector3 boxCastDirection = Vector3.down; // ������
            float boxCastDistance = 18f; // �K�v�ɉ����Ē���
            Quaternion boxCastRotation = Quaternion.identity;

            // �{�b�N�X�͈̔͂�`�� (�n�_)
            Gizmos.matrix = Matrix4x4.TRS(boxCastOrigin, boxCastRotation, _boxCastSize * 2);
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one); // ���S����ɃX�P�[���K�p

            // �{�b�N�X�̏I�_���v�Z
            Vector3 boxCastEnd = boxCastOrigin + boxCastDirection.normalized * boxCastDistance;

            // �L���X�g�̈ړ��͈͂�`��
            Gizmos.matrix = Matrix4x4.identity;
            Gizmos.DrawLine(boxCastOrigin, boxCastEnd);

            // �{�b�N�X�̏I�_�͈̔͂�`��
            Gizmos.matrix = Matrix4x4.TRS(boxCastEnd, boxCastRotation, _boxCastSize * 2);
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
        }
#endif
    }
}