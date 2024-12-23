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
        [SerializeField] Transform _spawnPos;
        [SerializeField] Animator _anim;
        [SerializeField] AnimationCurve _animationCurveX;
        [SerializeField] AnimationCurve _animationCurveY;
        [SerializeField] AnimationCurve _animationCurveZ;
        Vector3 _position;
        bool _isSpawned;
        float _animationTimer = 0;
        float _animationDuration = 1;

#if UNITY_EDITOR
        bool _isChecked;
#endif

        protected override void Start_S()
        {
            _timer = Time.time;
            _animationCurveX.AddKey(0, _spawnPos.position.x);
            _animationCurveZ.AddKey(0, _spawnPos.position.z);
        }

        protected override void Think() //UpDate �Ɠ��`
        {
            if (_isPaused || _isCoolTimed)
                _timer += Time.deltaTime;

            if (!_isPaused)
            {
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
                if (_isSpawned && _entityList.Count > 0)
                {
                    _animationTimer += Time.deltaTime;
                    var normalizedTime = Mathf.Clamp01(_animationTimer / _animationDuration);
                    var x = _animationCurveX.Evaluate(normalizedTime);
                    var y = _animationCurveY.Evaluate(normalizedTime);
                    var z = _animationCurveZ.Evaluate(normalizedTime);
                    _entityList[_entityList.Count - 1].transform.position = new Vector3(x, y, z);
                    if (_animationTimer > _animationDuration)
                    {
                        _animationTimer = 0;
                        _isSpawned = false;
                        _animationCurveX.RemoveKey(1);
                        _animationCurveZ.RemoveKey(1);
                    }
                }
            }
        }
        protected override async void Summon(Vector3 pos, int count)
        {
            if (_entityList.Count < count)
            {
                if (_entityPrefab is null) return;
                var entity = InstantiateAsync(_entityPrefab, transform, _spawnPos.position, Quaternion.identity);
                GameObject obj = (await entity)[0];
                _entityList.Add(obj);
                _isSpawned = true;
                _animationCurveX.AddKey(1, _position.x);
                _animationCurveZ.AddKey(1, _position.z);
                _anim.SetTrigger("Attack");
            }
        }
        override protected bool Check(Vector3 pos)
        {
            var size = BombRange * 5;
            Physics.BoxCast(pos + new Vector3(0, 8, 0), Vector3.one * size / 2, Vector3.down, out RaycastHit hit, Quaternion.identity, 18f);

            if (hit.collider != null)
            {
                return hit.collider.gameObject.layer == Mathf.Log(_groundLayer.value, 2);
#if UNITY_EDITOR
                _isChecked = true;
#endif
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
            Gizmos.color = _isChecked ? Color.red : Color.green;

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