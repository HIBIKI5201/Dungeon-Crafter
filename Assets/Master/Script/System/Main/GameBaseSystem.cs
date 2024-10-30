using DCFrameWork.SceneSystem;
using System.Collections;
using System.Collections.Generic;
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
        private UIManager_B _mainUIManager;
        #endregion

        private List<IPausable> _pausableList = new();

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
        }

        private void Start()
        {
            (GameSaveData gameData, SettingSaveData settingSaveData) data = SaveDataManager.Load();
            SaveDataManager.SaveData = data.gameData;
            SaveDataManager.SettingSaveData = data.settingSaveData;

            _audioManager = GetComponentInChildren<AudioManager>();
            (_audioManager is null).CheckLog("AudioManager��������܂���ł���");
            _mainUIManager = FindAnyObjectByType<UIManager_B>();
            (_mainUIManager is null).CheckLog("MainUIManager��������܂���ł���");

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
            system?.Init(this);
        }

        public void PlaySound(int index, SoundKind kind) => _audioManager?.PlaySound(index, kind);

        #region �|�[�Y
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
            if ((obj is null).CheckLog("Ipausable��null")) return;
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
        /// �|�[�Y���̏���������
        /// </summary>
        void Pause();

        /// <summary>
        /// �|�[�Y�������̏���������
        /// </summary>
        void Resume();
    }
}