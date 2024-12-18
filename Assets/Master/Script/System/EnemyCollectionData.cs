using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace DCFrameWork
{
    [CreateAssetMenu(fileName = "EnemyCollection", menuName = "CollectionData/EnemyCollection")]
    public class EnemyCollectionData : CollectionData_B
    {
        [SerializeField]
        List<EnemyCollection> _enemyCollection = new();

        public List<EnemyCollection> EnemyData { get => _enemyCollection; }

        public override int Count => _enemyCollection.Count;

        public override void ReadData(StringReader reader)
        {
            Debug.Log(reader.Peek());
            _enemyCollection.Clear();
            while (reader.Peek() != -1)
            {
                string line = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                string[] elements = line.Split(',').Select(s => s.Replace("\"", "").Trim()).ToArray();
                if (elements[0][0] == '/') continue;

                Debug.Log($"{elements[0]}{elements[1]}");
                _enemyCollection.Add(new EnemyCollection()
                {
                    _name = elements[0],
                    _tips = elements[1],
                    _sprite = LoadTexture(elements[2]),
                });
            }
            Debug.Log("End");
        }
    }

    [System.Serializable]
    public struct EnemyCollection
    {
        public string _name;
        public string _tips;
        public Texture2D _sprite;
        [HideInInspector] public int _killCount;
    }
#if UNITY_EDITOR
    [CustomEditor(typeof(EnemyCollectionData))]
    public class LoadEnemySpreadSheet : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            GUILayout.Space(10);
            var collection = target as EnemyCollectionData;

            if (GUILayout.Button("テキスト読み込み"))
            {
                Debug.Log("load");
                collection.LoadAndRead();
            }
        }
    }
#endif
}
