using DCFrameWork.MainSystem;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using DCFrameWork;

namespace DCFrameWork
{
    [CreateAssetMenu(fileName = "AudioCollection", menuName = "CollectionData/AudioCollection")]
    public class BGMCollectionData : CollectionData_B
    {
        [SerializeField]
        AudioDataBase _bgmData;
        [SerializeField]
        List<AudioCollection> _bgmCollection = new();

        public List<AudioCollection> BGMCollection { get => _bgmCollection; }

        public override int Count => _bgmCollection.Count;

        public override void ReadData(StringReader reader)
        {
            if ((_bgmData == null).CheckLog("bgmData is null"))return;
            Debug.Log(reader.Peek());
            _bgmCollection.Clear();
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
                _bgmCollection.Add(new AudioCollection()
                {
                    _name = elements[0],
                    _usedLocation = elements[1],
                    _audioData = _bgmData.audioDatas.Find(x => x.AudioClip.name == elements[0])
                });
            }
            Debug.Log("End");
        }
    }

    [System.Serializable]
    public struct AudioCollection
    {
        public string _name;
        public string _usedLocation;
        public AudioData _audioData;
    }
#if UNITY_EDITOR
    [CustomEditor(typeof(BGMCollectionData))]
    public class LoadBGMSpreadSheet : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            GUILayout.Space(10);
            var collection = target as BGMCollectionData;

            if (GUILayout.Button("テキスト読み込み"))
            {
                Debug.Log("load");
                collection.LoadAndRead();
            }
        }
    }
#endif
}
