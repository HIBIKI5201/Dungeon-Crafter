using System.Collections.Generic;
using UnityEngine;

namespace DCFrameWork.DefenseEquipment
{
    [CreateAssetMenu(menuName = "GameData/DefenseData/TurretDataBase", fileName = "DefebseEquipmentDataBase")]
    public class DefenseEquipmentDataBase : ScriptableObject
    {
        [Header("タレットの名前")]
        public string Name;
        [Header("タレットの種類")] 
        public DefenseObjectsKind Kind;
        [Header("タレットのプレハブ")] 
        public GameObject Prefab;
        [Header("タレットの説明文")] 
        public string Explanation;
        [Header("ガチャでのタレットの出現確率")] 
        public int DropChance;
        [Header("タレットのステータスリスト")] 
        public List<DefenseEquipmentData_B> DataLevelList;
        [Header("タレットのステータスリスト")]
        public List<int> PowerUpRequireItem;
    }
}
