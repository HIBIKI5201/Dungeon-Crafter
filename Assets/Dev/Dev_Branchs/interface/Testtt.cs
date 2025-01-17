using DCFrameWork.UI;
using UnityEngine;

namespace DCFrameWork
{
    public class Testtt : MonoBehaviour
    {
        GameObject _uiDocument;
        InGameUIManager _inGameUiManager;
        private void Start()
        {
            _uiDocument = GameObject.Find("UIDocument");
            _inGameUiManager = _uiDocument.GetComponent<InGameUIManager>();
        }
    }
}
