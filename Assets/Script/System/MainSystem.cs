using UnityEngine;

namespace DCFrameWork.MainSystem
{
    public abstract class MainSystem<T> : MonoBehaviour where T :MainSystem<T>
    {
        private static T _instance;

        protected static T MyInstance
        {
            get
            {
                if (_instance == null)
                {
                    Debug.LogError(typeof(T).Name + " is not initialized.");
                }
                return _instance;
            }
        }

        AudioManager _audioManager;

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = (T)this;
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

        }

        public void PlaySoundEffect(int index) => _audioManager?.PlaySoundEffect(index);
        
        public void PlayBGM(int index) => _audioManager.PlayBGM(index);
    }
}