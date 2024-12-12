
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

        [SerializeField] DefenseCollectionData _defenseObjCollectionData;
        [SerializeField] EnemyCollectionData _enemyCollectionData;
        [SerializeField] BGMCollectionData _bgmCollectionData;

        int _defenseObjectFrag;
        int _enemyFrag;
        int _bgmFrag;

        static Dictionary<EnemyKind, int> _enemyKillCount = new Dictionary<EnemyKind, int>();

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
        }
        public void SetSaveData(int defensObjectFrag, int enemyFrag, int bgmFrag, Dictionary<EnemyKind, int> enemyKillCount)
        {
            _defenseObjectFrag = defensObjectFrag;
            _enemyFrag = enemyFrag;
            _bgmFrag = bgmFrag;
            _enemyKillCount = enemyKillCount;
        }

        public IEnumerable<EnemyCollection?> GetEnemyCollection()
        {
            List<EnemyCollection?> data = new();
            if ((!_enemyCollectionData).CheckLog("EnemyCollectionData������܂���")) return data;
            for (var i = 0; i < _enemyCollectionData.Count; i++)
            {
                EnemyCollection? col = (_enemyFrag & 1 << i) != 0 ? _enemyCollectionData.EnemyData[i] : null;
                if (col != null)
                {
                    var j = col.Value;
                    j._killCount = _enemyKillCount[(EnemyKind)i];
                }
                data.Add(col);
            }
            return data;
        }
        public IEnumerable<DefenseCollection?> GetDefenseObjCollection()
        {
            List<DefenseCollection?> data = new();
            if ((!_defenseObjCollectionData).CheckLog("DefenseCollectionData������܂���")) return data;
            for (var i = 0; i < _defenseObjCollectionData.Count; i++)
            {
                data.Add((_defenseObjectFrag & 1 << i) != 0 ? _defenseObjCollectionData.DefenseData[i] : null);
            }
            return data;
        }
        public IEnumerable<AudioCollection?> GetBGMCollection()
        {
            List<AudioCollection?> data = new List<AudioCollection?>();
            if ((!_bgmCollectionData).CheckLog("bgmData������܂���")) return data;
            for (var i = 0; i < _bgmCollectionData.BGMCollection.Count; i++)
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
                Debug.Log(item != null ? item.Value._name : "null");
            }
            var k = GetEnemyCollection();
            foreach (var item in k)
            {
                Debug.Log(item != null ? item.Value._name : "null");
            }
            var j = GetBGMCollection();
            foreach (var item in j)
            {
                Debug.Log(item != null ? item.Value._name : "null");
            }
        }
        public void SetEnemy(EnemyKind kind) => _enemyFrag |= 1 << (int)kind;
        public void SetDefenseObj(DefenseObjectsKind kind) => _defenseObjectFrag |= 1 << (int)kind;
        public void SetBGM(int num) => _bgmFrag |= 1 << num;

        public static void AddEnemyKilCount(EnemyKind kind)
        {
            if(_enemyKillCount.ContainsKey(kind))
                _enemyKillCount[kind]++;
            else 
                _enemyKillCount.Add(kind, 1);
        }
    }
}
