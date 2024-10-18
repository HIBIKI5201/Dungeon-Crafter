using System;
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
        List<AudioData> _inGameSoundEffectList = new();
        [SerializeField]
        List<AudioData> _outGameSoundEffectList = new();
        [SerializeField]
        List<AudioData> _BGMList = new();

        private void Start()
        {
            if (_soundEffectSource is null)
                Debug.LogWarning($"{gameObject.name}のAudioManagerにSoundEffectSourceがアサインされていません");
            if (_BGMSource is null)
                Debug.LogWarning($"{gameObject.name}のAudioManagerにBGMSourceがアサインされていません");
        }

        public void PlaySound(int index, SoundKind Kind)
        {
            switch (Kind)
            {
                case SoundKind.InGame:
                    PlayInGameSoundEffect(index);
                    break;
                case SoundKind.OutGame:
                    PlayOutGameSoundEffect(index);
                    break;
                case SoundKind.BGM:
                    PlayBGM(index);
                    break;
            }
        }

        private void PlayBGM(int index)
        {
            AudioData? data = _BGMList[index];
            if (_BGMSource is null || data is not null) return;
            _BGMSource.Stop();
            _BGMSource.volume = data.Value.AudioVolume;
            _BGMSource.clip = data.Value.AudioClip;
            _BGMSource.Play();
        }

        private void PlayInGameSoundEffect(int index) =>
            _soundEffectSource?.PlayOneShot(_inGameSoundEffectList[index].AudioClip);

        private void PlayOutGameSoundEffect(int index) =>
            _soundEffectSource?.PlayOneShot(_outGameSoundEffectList[index].AudioClip);
    }

    public enum SoundKind
    {
        InGame,
        OutGame,
        BGM,
    }

    [Serializable]
    public struct AudioData
    {
        public AudioClip AudioClip;
        [Range(0, 1)]
        public float AudioVolume;
    }
}