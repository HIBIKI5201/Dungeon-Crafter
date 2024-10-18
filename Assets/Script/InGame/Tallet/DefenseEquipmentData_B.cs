using UnityEngine;

public abstract class DefenseEquipmentData_B : ScriptableObject
{
    public string EquipmentName;
    //この施設がレベルアップ時にドロップする確率
    public int DropChance;

    public float Attack;
    public float Rate;
    public float Range;
    public float Critical;
}
