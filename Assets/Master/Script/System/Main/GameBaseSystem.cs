using DCFrameWork.SceneSystem;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace DCFrameWork.MainSystem
{
    public class GameBaseSystem : MonoBehaviour
    {
        #region サービスロケーター
        public static GameBaseSystem mainSystem { get => _instance; }
        public static SceneSystem_B sceneSystem { get; private set; }
        #endregion

        #region メインシステム コンポーネント
        private static GameBaseSystem _instance;

        private AudioManager _audioManager;
        private MainUIManager _mainUIManager;
        #endregion
        #region pause
        private event Action OnPaused;
        private event Action OnResumed;
        #endregion
        private void Awake()
        {
            if (!_instance)
            {
                _instance = this;
                //DontDestroyOnLoad(_instance);

                Scene scene = SceneManager.CreateScene("SystemScene");
                SceneManager.MoveGameObjectToScene(gameObject, scene);
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            _audioManager = GetComponentInChildren<AudioManager>();
            (_audioManager is null).CheckLog("AudioManagerが見つかりませんでした");
            _mainUIManager = FindAnyObjectByType<MainUIManager>();
            (_mainUIManager is null).CheckLog("MainUIManagerが見つかりませんでした");
        }

        private void Start()
        {
            SaveDataManager.Load();

            SceneInit();
        }

        public void LoadScene(SceneKind kind)
        {
            StartCoroutine(SceneLoading<SceneSystem_B>(kind, null));
        }

        public void LoadScene<T>(SceneKind kind, Action<T> loadedAction) where T : SceneSystem_B
        {
            StartCoroutine(SceneLoading(kind, loadedAction));
        }

        private IEnumerator SceneLoading<T>(SceneKind kind, Action<T> action) where T : SceneSystem_B
        {
            yield return SceneChanger.LoadScene(kind);
            T system = SceneInit() as T;
            if (system is not null)
                action?.Invoke(system);
            else
                Debug.LogWarning("ロードされたシーンとシーンシステムが異なります");
        }

        private SceneSystem_B SceneInit()
        {
            SceneSystem_B system = FindAnyObjectByType<SceneSystem_B>();
            if ((system is null).CheckLog("シーンマネージャーが見つかりません")) return　null;
            sceneSystem = system;
            system?.Initialize();
            return sceneSystem;
        }

        public void PlaySound(int index, SoundKind kind) => _audioManager?.PlaySound(index, kind);

        #region ポーズ
        public void Pause() => OnPaused?.Invoke();
        public void Resume() => OnResumed?.Invoke();

        public void AddPausableObject(IPausable obj)
        {
            if ((obj is null).CheckLog("Ipausableはnull")) return;
            OnPaused += obj.Pause;
            OnResumed += obj.Resume;
        }
        public void RemovePausableObject(IPausable obj)
        {
            OnPaused -= obj.Pause;
            OnResumed -= obj.Resume;
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