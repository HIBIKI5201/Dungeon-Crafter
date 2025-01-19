using DCFrameWork.Enemy;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DCFrameWork
{
    [CreateAssetMenu(fileName = "EnemyCollection", menuName = "CollectionData/EnemyCollection")]
    public class EnemyCollectionData : ScriptableObject
    {
        [SerializeField]
        List<EnemyData_B> _enemyCollection = new();

        public List<EnemyData_B> EnemyData { get => _enemyCollection; }

        public int Count => _enemyCollection.Count;

#if UNITY_EDITOR
        public void LoadEnemy()
        {
            _enemyCollection.Clear();
            foreach (EnemyKind kind in Enum.GetValues(typeof(EnemyKind)))
            {
                _enemyCollection.Add(CollectionData_B.LoadAsset<EnemyData_B>("EnemyData" + kind));
            }
        }
#endif
        [Serializable]
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

                if (GUILayout.Button("�e�L�X�g�ǂݍ���"))
                {
                    Debug.Log("load");
                    collection.LoadEnemy();
                }
            }
        }
#endif
    }
}
