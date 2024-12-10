using DCFrameWork.MainSystem;
using System.IO;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

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
            //スプレッドシート読み込み
            UnityWebRequest request = UnityWebRequest.Get($"https://docs.google.com/spreadsheets/d/{link}/gviz/tq?tqx=out:csv&sheet={sheetName}");
            await request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(request.error);
                return null;
            }
            return new StringReader(request.downloadHandler.text);
        }
        protected Texture2D LoadTexture(string assetName)
        {
            if (string.IsNullOrWhiteSpace(assetName)) return null;
            string[] assetGUID = AssetDatabase.FindAssets(assetName);

            if ((assetGUID.Length == 0).CheckLog($"{assetName}が見つかりません")) return null;
            string path = AssetDatabase.GUIDToAssetPath(assetGUID[0]);

            Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
            if (texture != null)
            {
                return texture;
            }
            return null;
        }
        public abstract void ReadData(StringReader reader);
    }
}
