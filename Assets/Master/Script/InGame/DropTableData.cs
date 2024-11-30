using System.Collections.Generic;
using UnityEngine;

namespace DCFrameWork
{
    [CreateAssetMenu(fileName = "DropTableData", menuName = "GameData/DropTableData")]
    public class DropTableData : ScriptableObject
    {
        public List<DropData> _dropData = new();
        public IEnumerable<DefenseObjectsKind> GetRandomDefenseObj(int count)
        {
            List<DefenseObjectsKind> defenseObjects = new List<DefenseObjectsKind>();
            List<DropData> dropData = new List<DropData>(_dropData);
            Shuffle(dropData);
            dropData.RemoveRange(count, dropData.Count - count);
            Debug.Log(dropData.Count);
            return defenseObjects = dropData.ConvertAll(d => d._defenseObjectsKind);
        }
        void Shuffle<T>(List<T> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                int randomIndex = Random.Range(i, list.Count);
                (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
            }
        }
    }
    [System.Serializable]
    public struct DropData
    {
        public DefenseObjectsKind _defenseObjectsKind;
        public int _dropRate;
    }
}
