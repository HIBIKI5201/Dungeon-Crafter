using Unity.VisualScripting;
using UnityEngine;

namespace DCFrameWork.DefenseEquipment
{
    public class TrapTurretManager : DEEntityManager_SB<SummonData>
    {

        [SerializeField] float _minRange = 1;
        [SerializeField] Vector3 _boxCastSize = new Vector3(1, 10, 1);
        [SerializeField] LayerMask _groundLayer;
        Vector3 _position;
        float _timer = 0;
        bool _isPaused = false;
        int _maxCount;

        protected override void Start_S()
        {
            _timer = Time.time;
        }

        protected override void Think() //UpDate �Ɠ��`
        {
            if (_isPaused)
                _timer += Time.deltaTime;

            if (Time.time > 1 / DefenseEquipmentData.Rate + _timer)
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
                    Debug.LogError(Check(summonPos));
                    if (count <= 10)
                    {
                        isChecked = true;
                        break;
                    }
                }
                // Debug.Log(summonPos);
                if (!isChecked)
                    Summon(summonPos, _maxCount);
            }
        }
        bool Check(Vector3 pos)
        {
            Physics.BoxCast(pos, _boxCastSize / 2, Vector3.down, out RaycastHit hit, Quaternion.identity, 10f);

            if (hit.collider != null)
            {
                Debug.Log($"Hit object: {hit.collider.gameObject.name}, Layer: {hit.collider.gameObject.layer}");
                return hit.collider.gameObject.layer == Mathf.Log(_groundLayer.value, 2);
            }

            Debug.Log("No collision detected.");
            return false;
        }



        protected override void LoadSpecificData(SummonData data)
        {
            _maxCount = DefenseEquipmentData.MaxCount;
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Check(_position) ? Color.red : Color.green;

            // BoxCast �̃f�[�^���v�Z
            Vector3 boxCastOrigin = _position;
            Vector3 boxCastDirection = Vector3.down; // ������
            float boxCastDistance = 10f; // �K�v�ɉ����Ē���
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

        Vector3 SummonPosition()
        {
            float r = Random.Range(_minRange, DefenseEquipmentData.Range);
            float degree = Random.Range(0, 360);
            float radian = degree * Mathf.Deg2Rad;
            float randomPosX = r * Mathf.Cos(radian);
            float randomPosZ = r * Mathf.Sin(radian);
            return transform.position + new Vector3(randomPosX, 0, randomPosZ);
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