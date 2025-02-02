using DCFrameWork.SceneSystem;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace DCFrameWork.MainSystem
{
    public class GameBaseSystem : MonoBehaviour
    {
        #region �T�[�r�X���P�[�^�[
        public static GameBaseSystem mainSystem { get => _instance; }
        public static SceneSystem_B sceneSystem { get; private set; }
        #endregion

        #region ���C���V�X�e�� �R���|�[�l���g
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
                ToSingleton();
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            _audioManager = GetComponentInChildren<AudioManager>();
            if (_audioManager is null)
                Debug.LogWarning("AudioManager��������܂���ł���");
            _mainUIManager = FindAnyObjectByType<MainUIManager>();
            if (_mainUIManager is null)
                Debug.LogWarning("MainUIManager��������܂���ł���");

            async void ToSingleton()
            {
                _instance = this;

                string sceneName = SceneChanger.SetCurrentSceneName();

                while (!SceneManager.GetSceneByName(sceneName).isLoaded)
                {
                    await Awaitable.NextFrameAsync(); // ���̃t���[���܂őҋ@
                }

                SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
                Scene systemScene = SceneManager.CreateScene("SystemScene");
                SceneManager.MoveGameObjectToScene(gameObject, systemScene);
            }
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
                Debug.LogWarning("���[�h���ꂽ�V�[���ƃV�[���V�X�e�����قȂ�܂�");
        }

        private SceneSystem_B SceneInit()
        {
            SceneSystem_B system = FindAnyObjectByType<SceneSystem_B>();
            if ((system is null).CheckLog("�V�[���}�l�[�W���[��������܂���")) return null;
            sceneSystem = system;
            system?.Initialize();
            return sceneSystem;
        }

        public void PlaySound(int index, SoundKind kind) => _audioManager?.PlaySound(index, kind);

        public void PlayBGM(int index, BGMMode mode) => _audioManager?.PlayBGM(index, mode);

        #region �|�[�Y
        public static bool IsPause;
        public void Pause()
        {
            OnPaused?.Invoke();
            IsPause = true;
        }
        public void Resume()
        {
            OnResumed?.Invoke();
            IsPause = false;
        }

        public void AddPausableObject(IPausable obj)
        {
            if ((obj is null).CheckLog("Ipausable��null")) return;
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
        /// �|�[�Y���̏���������
        /// </summary>
        void Pause();

        /// <summary>
        /// �|�[�Y�������̏���������
        /// </summary>
        void Resume();
    }
}