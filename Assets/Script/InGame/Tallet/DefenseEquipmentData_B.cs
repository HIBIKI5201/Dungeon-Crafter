using UnityEngine;

public abstract class DefenseEquipmentData_B : ScriptableObject
{
    public string EquipmentName;
    //���̎{�݂����x���A�b�v���Ƀh���b�v����m��
    public int DropChance;

    public float Attack;
    public float Rate;
    public float Range;
    public float Critical;
}
