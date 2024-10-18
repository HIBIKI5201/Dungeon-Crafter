using System.Collections;
using UnityEngine;

namespace DCFrameWork.MainSystem
{
    public class MainSystem : MonoBehaviour
    {
        private static MainSystem _instance;

        AudioManager _audioManager;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }

        void Start()
        {
            _audioManager = GetComponentInChildren<AudioManager>();
            if (_audioManager is null)
                Debug.LogWarning("AudioManager‚ªŒ©‚Â‚©‚è‚Ü‚¹‚ñ‚Å‚µ‚½");
        }

        protected void SceneSystemInit()
        {

        }

        public void LoadScene(SceneKind kind)
        {
            StartCoroutine(SceneLoading(kind));
        }

        private IEnumerator SceneLoading(SceneKind kind)
        {
            yield return SceneChanger.LoadScene(kind);
        }

        public void SaveGameData() => SaveDataManager.Save();

        public void SaveSettingData() => SaveDataManager.SettingSave();

        public void PlaySoundEffect(int index) => _audioManager?.PlaySoundEffect(index);

        public void PlayBGM(int index) => _audioManager.PlayBGM(index);
    }
}