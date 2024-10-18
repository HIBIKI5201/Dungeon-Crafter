using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DCFrameWork.MainSystem
{
    public class MainSystem : MonoBehaviour
    {
        private static MainSystem _instance;
        
        AudioManager _audioManager;
        private UIManager_B _mainUIManager;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                Scene scene = SceneManager.CreateScene("SystemScene");
                SceneManager.MoveGameObjectToScene(gameObject, scene);
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }

        void Start()
        {
            (GameSaveData gameData, SettingSaveData settingSaveData) data = SaveDataManager.Load();
            SaveDataManager.SaveData = data.gameData;
            SaveDataManager.SettingSaveData = data.settingSaveData;

            _audioManager = GetComponentInChildren<AudioManager>();
            if (_audioManager is null)
                Debug.LogWarning("AudioManager‚ªŒ©‚Â‚©‚è‚Ü‚¹‚ñ‚Å‚µ‚½");
            _mainUIManager = GetComponentInChildren<UIManager_B>();
            if (_mainUIManager is null)
                Debug.LogWarning("MainUIManager‚ªŒ©‚Â‚©‚è‚Ü‚¹‚ñ‚Å‚µ‚½");
        }

        public void LoadScene(SceneKind kind)
        {
            StartCoroutine(SceneLoading(kind));
        }

        private IEnumerator SceneLoading(SceneKind kind)
        {
            yield return SceneChanger.LoadScene(kind);
            SceneSystem_B sceneSystem = FindAnyObjectByType<SceneSystem_B>();
            sceneSystem?.Init(this);
        }

        public void PlaySound(int index, SoundKind kind) => _audioManager?.PlaySound(index, kind);
    }
}