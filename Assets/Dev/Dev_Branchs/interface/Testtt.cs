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
        void Update()
        {
            if (Input.GetKeyDown("k"))
            {
                _inGameUiManager.WaveGuageUpdate(321, 0.5f);
                _inGameUiManager.EquipmentSettingWindowVisible(true);
            }
        }
    }
}
