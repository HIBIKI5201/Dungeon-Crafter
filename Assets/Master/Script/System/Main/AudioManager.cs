using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
namespace DCFrameWork.MainSystem
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField]
        private AudioSource _soundEffectSource;
        [SerializeField]
        private AudioSource _BGMSource;
        [SerializeField]
        private AudioSource _voiceSource;

        [Space]

        [SerializeField]
        private AudioMixer _mixer;

        [Space]

        [SerializeField]
        private AudioDataBase _inGameSoundEffectData;
        [SerializeField]
        private AudioDataBase _outGameSoundEffectData;
        [SerializeField]
        private AudioDataBase _BGMData;
        [SerializeField]
        private AudioDataBase _voiceData;

        [Space]

        [SerializeField]
        private float _changeBGMFadeTime=1;

        private void Start()
        {
            (_soundEffectSource is null).CheckLog($"{gameObject.name}��AudioManager��SoundEffectSource���A�T�C������Ă��܂���");
            (_BGMSource is null).CheckLog($"{gameObject.name}��AudioManager��BGMSource���A�T�C������Ă��܂���");
        }

        public void PlaySound(int index, SoundKind Kind)
        {
            switch (Kind)
            {
                case SoundKind.InGame:
                    PlaySoundEffect(GetAudioData(_inGameSoundEffectData.audioDatas, index));
                    break;
                case SoundKind.OutGame:
                    PlaySoundEffect(GetAudioData(_outGameSoundEffectData.audioDatas, index));
                    break;
                case SoundKind.BGM:
                    PlayBGM(GetAudioData(_BGMData.audioDatas, index));
                    break;
                case SoundKind.Voice:
                    PlayVoiceSound(GetAudioData(_voiceData.audioDatas, index));
                    break;
            }
        }

        public void PlayBGM(int index, BGMMode mode)
        {
            var data = GetAudioData(_BGMData.audioDatas, index);
            if ((_BGMSource is null || data.Value.AudioClip is null).CheckLog("BGM�̍Đ��Ŗ�肪�������܂���")) return;
            switch (mode)
            {
                case BGMMode.Fade:
                    StartCoroutine(FadeBGM(data));
                    break;
                case BGMMode.Stop:
                    _BGMSource.Stop();
                    break;
                case BGMMode.Default:
                    _BGMSource.Stop();
                    _BGMSource.volume = data.Value.AudioVolume;
                    _BGMSource.clip = data.Value.AudioClip;
                    _BGMSource.Play();
                    break;
            }
        }

        private AudioData? GetAudioData(List<AudioData> list, int index)
        {
            if (list.Count < index) return null;
            return list[index];
        }

        private void PlayBGM(AudioData? data)
        {
            if ((_BGMSource is null || data.Value.AudioClip is null).CheckLog("BGM�̍Đ��Ŗ�肪�������܂���")) return;
            if (_BGMSource.isPlaying)
            {
                StartCoroutine(FadeBGM(data));
            }
            else
            {
                _BGMSource.Stop();
                _BGMSource.volume = data.Value.AudioVolume;
                _BGMSource.clip = data.Value.AudioClip;
                _BGMSource.Play();
            }
        }
        private IEnumerator FadeBGM(AudioData? data)
        {
            float playingBGMVol = _BGMSource.volume;
            for (float time = 0; time < _changeBGMFadeTime; time += Time.deltaTime)
            {
                _BGMSource.volume = Mathf.Lerp(playingBGMVol, 0, time / _changeBGMFadeTime);
                yield return null;
            }
            _BGMSource.Stop();
            _BGMSource.volume = data.Value.AudioVolume;
            _BGMSource.clip = data.Value.AudioClip;
            _BGMSource.Play();

        }

        private void PlaySoundEffect(AudioData? data)
        {
            if ((_soundEffectSource is null || data.Value.AudioClip is null).CheckLog("SE�̍Đ��Ŗ�肪�������܂���")) return;
            _soundEffectSource.volume = data.Value.AudioVolume;
            _soundEffectSource.PlayOneShot(data.Value.AudioClip);
        }

        private void PlayVoiceSound(AudioData? data)
        {
            if ((_voiceSource is null || data.Value.AudioClip is null).CheckLog("Voice�̍Đ��Ŗ�肪�������܂���")) return;
            _voiceSource.volume = data.Value.AudioVolume;
            _voiceSource?.PlayOneShot(data.Value.AudioClip);
        }
    }

    public enum SoundKind
    {
        InGame,
        OutGame,
        BGM,
        Voice,
    }
    public enum BGMMode
    {
        Stop,
        Fade,
        Default,
    }

    [Serializable]
    public struct AudioData
    {
        public AudioClip AudioClip;
        [Range(0, 1)]
        public float AudioVolume;
    }
}