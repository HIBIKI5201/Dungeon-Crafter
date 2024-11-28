using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

namespace DCFrameWork.DefenseEquipment
{
    public class SummonTurretManager : DEEntityManager_SB<SummonData>
    {
        float _timer = 0;
        bool _isPaused = false;
        int _maxCount;
        [SerializeField] Transform _summonPosition;
        protected override void Start_S()
        {
            _timer = Time.time;
        }
        protected override void Think() //UpDate ‚Æ“¯‹`
        {
            if (_isPaused)
                _timer += Time.deltaTime;

            if (Time.time > 1 / DefenseEquipmentData.Rate + _timer)
            {
                Summon(_summonPosition.position, _maxCount);
                _timer = Time.time;
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

    }
}