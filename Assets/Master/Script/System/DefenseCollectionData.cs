using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace DCFrameWork
{
    [CreateAssetMenu(fileName = "DefenseCollection", menuName = "CollectionData/DefenseCollection")]
    public class DefenseCollectionData : CollectionData_B
    {
        [SerializeField]
        List<DefenseCollection> _defenseCollections;
        public List<DefenseCollection> DefenseData { get => _defenseCollections; }

        public override int Count => _defenseCollections.Count;

        public override void ReadData(StringReader reader)
        {
            _defenseCollections.Clear();
            while (reader.Peek() != -1)
            {
                string line = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                string[] elements = line.Split(',').Select(s => s.Replace("\"", "").Trim()).ToArray();
                if (elements[0][0] == '/') continue;
                Debug.Log(elements.Count());
                Debug.Log($"{elements[0]}{elements[1]}{elements[2]}{elements[3]}");
                _defenseCollections.Add(new DefenseCollection()
                {
                    _name = elements[0],
                    _tips = elements[1],
                    _cardSprite = LoadTexture(elements[2]),
                    //_objectSprite = LoadTexture(elements[3]),
                });
            }
            Debug.Log("End");
        }
    }
    [System.Serializable]
    public struct DefenseCollection
    {
        public string _name;
        public string _tips;
        public Texture2D _cardSprite;
        public Texture2D _objectSprite;
    }
#if UNITY_EDITOR
    [CustomEditor(typeof(DefenseCollectionData))]
    public class LoadDefenseSpreadSheet : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            GUILayout.Space(10);
            var collection = target as DefenseCollectionData;

            if (GUILayout.Button("テキスト読み込み"))
            {
                Debug.Log("load");
                collection.LoadAndRead();
            }
        }
    }
#endif
}
