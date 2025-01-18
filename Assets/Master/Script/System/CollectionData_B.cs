using DCFrameWork.MainSystem;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DCFrameWork
{
    public abstract class CollectionData_B : ScriptableObject
    {
        [SerializeField]
        string _link;
        [SerializeField]
        string _sheetName;

        public abstract int Count { get; }
        public async Task LoadAndRead() => ReadData(await LoadSpreadSheet(_link, _sheetName));
        async Task<StringReader> LoadSpreadSheet(string link, string sheetName)
        {
            //�X�v���b�h�V�[�g�ǂݍ���
            UnityWebRequest request = UnityWebRequest.Get($"https://docs.google.com/spreadsheets/d/{link}/gviz/tq?tqx=out:csv&sheet={sheetName}");
            await request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(request.error);
                return null;
            }
            return new StringReader(request.downloadHandler.text);
        }
        #if UNITY_EDITOR
        public static T LoadAsset<T>(string assetName) where T : Object
        {
            if (string.IsNullOrWhiteSpace(assetName)) return default;
            string[] assetGUID = AssetDatabase.FindAssets(assetName);

            if ((assetGUID.Length == 0).CheckLog($"{assetName}��������܂���")) return default;
            string path = AssetDatabase.GUIDToAssetPath(assetGUID[0]);

            T asset = AssetDatabase.LoadAssetAtPath<T>(path);
            if (asset != null)
            {
                return asset;
            }
            return default;
        }
        #endif
        public abstract void ReadData(StringReader reader);
    }
}
