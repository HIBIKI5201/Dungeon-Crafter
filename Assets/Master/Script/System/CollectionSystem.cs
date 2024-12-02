
using DCFrameWork.Enemy;
using DCFrameWork.MainSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DCFrameWork
{
    public class CollectionSystem : MonoBehaviour
    {
        public static CollectionSystem Instans;

        [SerializeField] List<CollectionData> _defenseObjCollectionData=new();
        [SerializeField] List<CollectionData> _enemyCollectionData=new();
        [SerializeField] AudioDataBase _bgmCollectionData;

        int _defenseObjectFrag;
        int _enemyFrag;
        int _bgmFrag;

        /// <summary>
        /// ê}ä”ÇÃêiíª
        /// </summary>
        public float CollectionRate
        {
            get => (float)(Convert.ToString(_defenseObjectFrag, 2).Count(c => c == '1') + Convert.ToString(_enemyFrag, 2).Count(c => c == '1') +
                Convert.ToString(_bgmFrag, 2).Count(c => c == '1')) / (_defenseObjCollectionData.Count + _enemyCollectionData.Count + _bgmCollectionData.audioDatas.Count);
        }

        private void Awake()
        {
            if (Instans != null)
            {
                Destroy(gameObject);
            }
            Instans = this;
            DontDestroyOnLoad(gameObject);
        }

        public IEnumerable<CollectionData?> GetEnemyCollection()
        {
            List<CollectionData?> data = new List<CollectionData?>();
            for (var i = 0; i < _enemyCollectionData.Count; i++)
            {
                data.Add((_enemyFrag & 1 << i) != 0 ? _enemyCollectionData[i] : null);
            }
            return data;
        }

        public IEnumerable<CollectionData> GetDefenseObjCollection()
        {
            List<CollectionData> data = new List<CollectionData>();
            for (var i = 1; i < _defenseObjCollectionData.Count; i++)
            {
                data.Add((_defenseObjectFrag & 1 << i - 1) != 0 ? _defenseObjCollectionData[i + 1] : _defenseObjCollectionData[0]);
            }
            return data;
        }
        public IEnumerable<CollectionData> GetBGMCollection()
        {
            List<CollectionData> data = new List<CollectionData>();
            for (var i = 1; i < _defenseObjCollectionData.Count; i++)
            {
                data.Add((_defenseObjectFrag & 1 << i - 1) != 0 ? _defenseObjCollectionData[i + 1] : _defenseObjCollectionData[0]);
            }
            return data;
        }
        public void SetEnemy(EnemyKind kind) => _enemyFrag |= 1 << (int)kind;
        public void SetDefenseObj(DefenseObjectsKind kind) => _defenseObjectFrag |= 1 << (int)kind;

        public void SetBGM(int num) => _bgmFrag |= 1 << num;

        [ContextMenu("TestCollection")]
        public void TestCollection()
        {
            var a = GetEnemyCollection();
            foreach (var item in a)
            {
                Debug.Log(item?._name);
            }
        }
        [ContextMenu("SetEnemy")]
        public void TestEnemySet()
        {
            EnemyKind kind = EnemyKind.Normal;
            for (var i = 0; i < _enemyCollectionData.Count - 1; i++)
            {
                int n = UnityEngine.Random.Range(0, 2);
                if (n == 1) SetEnemy(kind++);
            }
            Debug.Log($"Frag : {Convert.ToString(_enemyFrag, 2)}");
        }
        [ContextMenu("collection")]
        public void TestLog()
        {
            Debug.Log(CollectionRate);
        }
    }
    [System.Serializable]
    public struct CollectionData
    {
        public string _name;
        public string _tips;
        public Sprite _sprite;
    }
}
