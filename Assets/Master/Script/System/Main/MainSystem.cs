using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DCFrameWork.MainSystem
{
    public class MainSystem : MonoBehaviour
    {
        public static MainSystem mainSystem { get => _instance; }
        public static SceneSystem_B sceneSystem { get; private set; }

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
                Debug.LogWarning("AudioManagerが見つかりませんでした");
            _mainUIManager = GetComponentInChildren<UIManager_B>();
            if (_mainUIManager is null)
                Debug.LogWarning("MainUIManagerが見つかりませんでした");
        }

        public void LoadScene(SceneKind kind)
        {
            StartCoroutine(SceneLoading(kind));
        }

        private IEnumerator SceneLoading(SceneKind kind)
        {
            yield return SceneChanger.LoadScene(kind);
            SceneSystem_B system = FindAnyObjectByType<SceneSystem_B>();
            sceneSystem = system;
            system?.Init(this);
        }

        public void PlaySound(int index, SoundKind kind) => _audioManager?.PlaySound(index, kind);

        #region フレームワーク
        /// <summary>
        /// 条件が揃った場合にコメントをデバッグログに出力する
        /// </summary>
        /// <param name="func">条件式</param>
        /// <param name="comment">ログのコメント</param>
        /// <returns>条件が揃っているか</returns>
        public static bool NullChecker(Func<bool> func, string comment)
        {
            bool result = func();
            if (result) Debug.Log(comment);
            return result;
        }
        #endregion
    }
}