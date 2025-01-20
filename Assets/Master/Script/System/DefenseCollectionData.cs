using DCFrameWork.DefenseEquipment;
using DCFrameWork.Enemy;
using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace DCFrameWork
{
    [CreateAssetMenu(fileName = "DefenseCollection", menuName = "CollectionData/DefenseCollection")]
    public class DefenseCollectionData : ScriptableObject
    {
        [SerializeField]
        List<DefenseEquipmentDataBase> _defenseCollections;
        public List<DefenseEquipmentDataBase> DefenseData { get => _defenseCollections; }

        public int Count => _defenseCollections.Count;

#if UNITY_EDITOR
        public void LoadDefenseObj()
        {
            _defenseCollections.Clear();
            foreach (DefenseObjectsKind kind in Enum.GetValues(typeof(EnemyKind)))
            {
                _defenseCollections.Add(CollectionData_B.LoadAsset<DefenseEquipmentDataBase>(kind + "DataBase"));
            }
        }
#endif
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

            if (GUILayout.Button("�e�L�X�g�ǂݍ���"))
            {
                Debug.Log("load");
                collection.LoadDefenseObj();
            }
        }
    }
#endif
}
