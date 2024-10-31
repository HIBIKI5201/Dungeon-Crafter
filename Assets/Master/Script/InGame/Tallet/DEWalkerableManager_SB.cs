using DCFrameWork.DefenseEquipment;
using DCFrameWork.Enemy;

public abstract class DEWalkerableManager_SB<Data> : DefenseEquipmentManager_B<Data> where Data : DefenseEquipmentData_B
{
    protected void TargetAddCondition(IConditionable enemy, ConditionType type)
    {
        enemy.AddCondition(type);
    }

    protected void TargetRemoveCondition(IConditionable enemy, ConditionType type)
    {
        enemy.RemoveCondition(type);
    }
}