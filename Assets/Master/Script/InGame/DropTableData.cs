using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DCFrameWork
{
    [CreateAssetMenu(fileName = "DropTableData", menuName = "GameData/DropTableData")]
    public class DropTableData : ScriptableObject
    {
        public List<DropData> _dropData = new();
        public IEnumerable<DefenseObjectsKind> GetRandomDefenseObj(int count)
        {
            List<DropData> dropData = new List<DropData>(_dropData);
            List<DropData> outData = new();

            for (int i = 0; i < count; i++)
            {
                int rateCount = 0;
                for (int j = 0; j < i; j++)
                {
                    rateCount += dropData[j]._dropRate;
                }
                int randomIndex = Random.Range(rateCount, dropData.Sum(x=>x._dropRate));
                for (int j = 0,k=0; k<dropData.Count; k++)
                {
                    j += dropData[k]._dropRate;
                    if (j >= randomIndex)
                    {
                        (dropData[i], dropData[k]) = (dropData[k], dropData[i]);
                    }
                }
            }
            dropData.RemoveRange(count, dropData.Count - count);
            return dropData.ConvertAll(d => d._defenseObjectsKind);
        }
        void Shuffle(List<DropData> list)
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
