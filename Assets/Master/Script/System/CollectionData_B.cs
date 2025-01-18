using System.IO;
using System.Threading.Tasks;
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
        public abstract void ReadData(StringReader reader);
    }
}
