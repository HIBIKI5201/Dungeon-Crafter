using System.Collections.Generic;
using UnityEngine;
namespace DCFrameWork.MainSystem
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField]
        private AudioSource _soundEffectSource;
        [SerializeField]
        private AudioSource _BGMSource;

        [SerializeField]
        List<AudioClip> _soundEffectList = new();
        [SerializeField]
        List<AudioClip> _BGMList = new();

        private void Start()
        {
            if(_soundEffectSource is null)
                Debug.LogWarning($"{gameObject.name}のAudioManagerにSoundEffectSourceがアサインされていません");
            if( _BGMSource is null)
                Debug.LogWarning($"{gameObject.name}のAudioManagerにBGMSourceがアサインされていません");
        }

        public void PlayBGM(int index)
        {
            if (_BGMSource is null) return;
            _BGMSource.Stop();
            _BGMSource.clip = _BGMList[index];
            _BGMSource.Play();
        }

        public void PlaySoundEffect(int index)
        {
            _soundEffectSource?.PlayOneShot(_soundEffectList[index]);
        }
    }
}