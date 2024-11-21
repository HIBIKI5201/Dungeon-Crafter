using DCFrameWork.SceneSystem;
using System;
using System.Collections;
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
            (_audioManager is null).CheckLog("AudioManager��������܂���ł���");
            _mainUIManager = FindAnyObjectByType<MainUIManager>();
            (_mainUIManager is null).CheckLog("MainUIManager��������܂���ł���");
        }

        private void Start()
        {
            SaveDataManager.Load();

            SceneInit();
        }

        public void LoadScene(SceneKind kind)
        {
            StartCoroutine(SceneLoading(kind));
        }

        private IEnumerator SceneLoading(SceneKind kind)
        {
            yield return SceneChanger.LoadScene(kind);
            SceneInit();
        }

        private void SceneInit()
        {
            SceneSystem_B system = FindAnyObjectByType<SceneSystem_B>();
            if ((system is null).CheckLog("�V�[���}�l�[�W���[��������܂���")) return;
            sceneSystem = system;
            system?.Initialize();
        }

        public void PlaySound(int index, SoundKind kind) => _audioManager?.PlaySound(index, kind);

        #region �|�[�Y
        public void Pause() => OnPaused?.Invoke();
        public void Resume() => OnResumed?.Invoke();

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