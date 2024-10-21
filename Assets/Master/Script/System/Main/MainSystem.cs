using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DCFrameWork.MainSystem
{
    public class MainSystem : MonoBehaviour
    {
        #region サービスロケーター
        public static MainSystem mainSystem { get => _instance; }
        public static SceneSystem_B sceneSystem { get; private set; }
        #endregion

        #region メインシステム コンポーネント
        private static MainSystem _instance;

        private AudioManager _audioManager;
        private UIManager_B _mainUIManager;
        #endregion

        private List<IPausable> _pausableList = new();

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

        private void Start()
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

        #region ポーズ
        public void Pause()
        {
            _pausableList?.ForEach(p => p.Pause());
        }

        public void Resume()
        {
            _pausableList?.ForEach(p => p.Resume());
        }

        public void AddPausableObject(IPausable obj)
        {
            if (!_pausableList?.Contains(obj) ?? false)
            {
                _pausableList.Add(obj);
            }
        }
        public void RemovePausableObject(IPausable obj)
        {
            if (_pausableList?.Contains(obj) ?? false)
            {
                _pausableList.Remove(obj);
            }
        }
        #endregion
    }

    public interface IPausable
    {
        /// <summary>
        /// ポーズ時の処理を実装
        /// </summary>
        void Pause();

        /// <summary>
        /// ポーズ解除時の処理を実装
        /// </summary>
        void Resume();
    }
}