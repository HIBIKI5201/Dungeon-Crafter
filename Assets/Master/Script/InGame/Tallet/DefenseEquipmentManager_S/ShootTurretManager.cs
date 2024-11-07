using DCFrameWork;
using DCFrameWork.DefenseEquipment;
using DCFrameWork.Enemy;
using DCFrameWork.MainSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootTurretManager : DEAttackerManager_SB<AttackebleData>
{
    private float _range;

    const float _interval = 1;
    float _timer = 0;
    bool _isPaused = false;
    protected override void Think() //UpDate ‚Æ“¯‹`
    {
        if (!_isPaused)
        {
            var attackRate = 1 / _rate * Time.deltaTime;
            _timer += attackRate;
            if (_timer > _interval)
            {
                Attack();
                _timer = 0;
            }
        }
    }
    protected override void LoadSpecificData(AttackebleData data)
    {
        _range = data.Range;
        RangeUp(_range);
    }
    protected override void Attack()
    {
        Debug.Log("Attack is Now");
        var criticalPoint = Random.Range(0, 100);
        var targetSelect = TargetSelect();
        Debug.Log(targetSelect.Count);
        TargetsAddDamage(targetSelect, criticalPoint <= _critical ? _attack * 3 : _attack);
    }

    void RangeUp(float range)
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
