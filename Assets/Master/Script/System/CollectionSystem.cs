
using DCFrameWork.Enemy;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DCFrameWork
{
    public class CollectionSystem : MonoBehaviour
    {
        public static CollectionSystem Instans;

        [SerializeField] DefenseCollectionData _defenseObjCollectionData;
        [SerializeField] EnemyCollectionData _enemyCollectionData = new();
        [SerializeField] BGMCollectionData _bgmCollectionData;

        int _defenseObjectFrag;
        int _enemyFrag;
        int _bgmFrag;

        public int _defenseObjectFrag2;
        public int _enemyFrag2;
        public int _bgmFrag2;

        Dictionary<EnemyKind, int> _EnemyKillCount = new Dictionary<EnemyKind, int>();

        /// <summary>
        /// �}�ӂ̐i��
        /// </summary>
        public float CollectionRate
        {
            get => (float)(Convert.ToString(_defenseObjectFrag, 2).Count(c => c == '1') +
                Convert.ToString(_enemyFrag, 2).Count(c => c == '1') +
                Convert.ToString(_bgmFrag, 2).Count(c => c == '1')) / (_defenseObjCollectionData.Count +
                _enemyCollectionData.EnemyData.Count + _bgmCollectionData.BGMCollection.Count);
        }

        private void Awake()
        {
            if (Instans != null)
            {
                Destroy(gameObject);
            }
            Instans = this;
            DontDestroyOnLoad(gameObject);
            _defenseObjectFrag = _defenseObjectFrag2;
            _enemyFrag = _enemyFrag2;
            _bgmFrag = _bgmFrag2;
        }
        public void SetSaveData(int defensObjectFrag, int enemyFrag, int bgmFrag, Dictionary<EnemyKind, int> enemyKillCount)
        {
            _defenseObjectFrag = defensObjectFrag;
            _enemyFrag = enemyFrag;
            _bgmFrag = bgmFrag;
            _EnemyKillCount = enemyKillCount;
        }

        public IEnumerable<EnemyCollection?> GetEnemyCollection()
        {
            List<EnemyCollection?> data = new ();
            for (var i = 0; i < _enemyCollectionData.Count; i++)
            {
                data.Add((_enemyFrag & 1 << i) != 0 ? _enemyCollectionData.EnemyData[i] : null);
                data[i]._killCount = _EnemyKillCount[(EnemyKind)i];
            }
            return data;
        }
        public IEnumerable<DefenseCollection?> GetDefenseObjCollection()
        {
            List<DefenseCollection?> data = new();
            for (var i = 1; i < _defenseObjCollectionData.Count; i++)
            {
                data.Add((_defenseObjectFrag & 1 << i) != 0 ? _defenseObjCollectionData.DefenseData[i] : null);
            }
            return data;
        }


        public IEnumerable<AudioCollection?> GetBGMCollection()
        {
            List<AudioCollection?> data = new List<AudioCollection?>();
            for (var i = 1; i < _bgmCollectionData.BGMCollection.Count; i++)
            {
                data.Add((_bgmFrag & 1 << i) != 0 ? _bgmCollectionData.BGMCollection[i] : null);
            }
            return data;
        }
        [ContextMenu("test")]
        public void Test()
        {
            var i = GetDefenseObjCollection();
            foreach (var item in i)
            {
                Debug.Log(item!=null?item.Value._name:"null");
            }
            var k = GetDefenseObjCollection();
            foreach (var item in k)
            {
                Debug.Log(item!=null? item.Value._name:"null");
            }
            var j = GetDefenseObjCollection();
            foreach (var item in j)
            {
                Debug.Log(item != null ? item.Value._name : "null");
            }
        }
        public void SetEnemy(EnemyKind kind) => _enemyFrag |= 1 << (int)kind;
        public void SetDefenseObj(DefenseObjectsKind kind) => _defenseObjectFrag |= 1 << (int)kind;
        public void SetBGM(int num) => _bgmFrag |= 1 << num;

        public void AddEnemyKilCount(EnemyKind kind)
        {
            _EnemyKillCount[kind]++;
        }
    }
}
