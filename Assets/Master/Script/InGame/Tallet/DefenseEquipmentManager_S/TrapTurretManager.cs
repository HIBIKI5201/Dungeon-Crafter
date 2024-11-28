using Unity.VisualScripting;
using UnityEngine;
namespace DCFrameWork.DefenseEquipment
{
    public class TrapTurretManager : DEEntityManager_SB<SummonData>
    {

        float _timer = 0;
        bool _isPaused = false;
        int _maxCount;
#if UNITY_EDITOR
        [SerializeField] Vector3 _boxCastSize = new(1, 10, 1);
#endif
        Vector3 _position;

        protected override void Start_S()
        {
            _timer = Time.time;
        }

        protected override void Think() //UpDate と同義
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
                if (!isChecked)
                    Summon(summonPos, _maxCount);
            }
        }

        protected override void LoadSpecificData(SummonData data)
        {
            _maxCount = DefenseEquipmentData.MaxCount;
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

            // BoxCast のデータを計算
            Vector3 boxCastOrigin = _position + new Vector3(0, 8, 0);
            Vector3 boxCastDirection = Vector3.down; // 下向き
            float boxCastDistance = 18f; // 必要に応じて調整
            Quaternion boxCastRotation = Quaternion.identity;

            // ボックスの範囲を描画 (始点)
            Gizmos.matrix = Matrix4x4.TRS(boxCastOrigin, boxCastRotation, _boxCastSize * 2);
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one); // 中心を基準にスケール適用

            // ボックスの終点を計算
            Vector3 boxCastEnd = boxCastOrigin + boxCastDirection.normalized * boxCastDistance;

            // キャストの移動範囲を描画
            Gizmos.matrix = Matrix4x4.identity;
            Gizmos.DrawLine(boxCastOrigin, boxCastEnd);

            // ボックスの終点の範囲を描画
            Gizmos.matrix = Matrix4x4.TRS(boxCastEnd, boxCastRotation, _boxCastSize * 2);
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
        }
#endif
    }
}