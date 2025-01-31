using DCFrameWork.DefenseEquipment;
using UnityEngine;

namespace DCFrameWork
{
    public class LongRangeBulletAnim : MonoBehaviour
    {
        [SerializeField] LongRangeShootTurretManager _turret;
        void BulletChange()//AnimationEventで使用しています
        {
            _turret.BulletShoot(false);
        }
    }
}
