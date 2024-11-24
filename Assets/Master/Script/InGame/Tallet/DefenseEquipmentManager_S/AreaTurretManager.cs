using DCFrameWork.Enemy;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DCFrameWork.DefenseEquipment
{
    public class AreaTurretManager : DefenseEquipmentManager_B<DefenseEquipmentData_B>
    {
        bool _isPaused = false;

        protected override void Think() //UpDate ‚Æ“¯‹`
        {
            
        }

        protected override void LoadSpecificData(DefenseEquipmentData_B data)
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

    }
}