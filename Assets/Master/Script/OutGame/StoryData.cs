using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;




#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DCFrameWork.SceneSystem
{
    [CreateAssetMenu(menuName = "GameData/StoryTextData", fileName = "StoryTextData")]
    public class StoryData : ScriptableObject
    {
        private const string _sheet_link = "1c7ofmhmkiQzzynmyA9jbkyiZgM5FQ7a_TuC6zxNeGIQ";

        [SerializeField, Tooltip("スプレッドシートのページ名")]
        private string _sheetName;

        [SerializeField]
        private Sprite _backGround;
        public Sprite BackGround { get => _backGround; }

        [SerializeField]
        private List<StoryText> _list = new();
        public List<StoryText> StoryText { get => _list;  }
#if UNITY_EDITOR
        public async Task Method()
        {
            //スプレッドシート読み込み
            UnityWebRequest request = UnityWebRequest.Get("https://docs.google.com/spreadsheets/d/" + _sheet_link + "/gviz/tq?tqx=out:csv&sheet=" + _sheetName);
            await request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(request.error);
                return;
            }

            // リスト生成
            _list.Clear();
            var reader = new StringReader(request.downloadHandler.text);

            while (reader.Peek() != -1)
            {
                string line = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(line)) continue;

                // 行をカンマで分割
                string[] elements = line.Split(',').Select(s => s.Replace("\"", "").Replace('/', '\n').Trim()).ToArray();
                if (int.Parse(elements[4]) == 0 && string.IsNullOrEmpty(elements[3])) {
                    break;
                }

                _list.Add(new StoryText(elements[0], elements[1], elements[2]));
            }
        }
#endif
    }

    [Serializable]
    public class StoryText
    {
        [SerializeField]
        private string _character;
        public string Character { get => _character; }
        
        [SerializeField, TextArea]
        private string _text;
        public string Text {  get => _text; }

        [SerializeField]
        private string _animation;
        public string Animation { get => _animation; }

        [SerializeField]
        private AudioClip _audioClip;
        public AudioClip AudioClip { get => _audioClip; }

        public StoryText(string character, string text, string animation)
        {
            _character = character;
            _text = text;
            _animation = animation;
        }
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(StoryData))]
    public class MyScriptEditor : Editor
    {
        public override async void OnInspectorGUI()
        {
            DrawDefaultInspector();

            GUILayout.Space(10);
            var storyData = target as StoryData;

            if (GUILayout.Button("テキスト読み込み"))
            {
                await storyData.Method();
            }
        }
    }
#endif
}
