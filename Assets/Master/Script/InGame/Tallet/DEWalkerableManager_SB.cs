using DCFrameWork.DefenseEquipment;
using DCFrameWork.Enemy;

public abstract class DEWalkerableManager_SB : DefenseEquipmentData_B
{
    protected void TargetAddCondition(EnemyManager_B enemy, ConditionType type)
    {
        enemy.AddCondition(type);
    }

    protected void TargetRemoveCondition(EnemyManager_B enemy, ConditionType type)
    {
        enemy.RemoveCondition(type);
    }
}