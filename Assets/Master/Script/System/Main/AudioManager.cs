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
        private AudioSource _defaultBGMSource;
        [SerializeField]
        private AudioSource _fadeBGMSource;
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
        private float _changeBGMFadeTime = 1;

        Coroutine _fadeCoroutine;
        private void Start()
        {
            (_soundEffectSource is null).CheckLog($"{gameObject.name}��AudioManager��SoundEffectSource���A�T�C������Ă��܂���");
            (_defaultBGMSource is null).CheckLog($"{gameObject.name}��AudioManager��BGMSource���A�T�C������Ă��܂���");
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
            if (!_defaultBGMSource || !_fadeBGMSource) return;
            if (_fadeCoroutine != null)
            {
                StopCoroutine( _fadeCoroutine );
                _fadeBGMSource.Stop();
            }
            var data = GetAudioData(_BGMData.audioDatas, index);
            AudioData? playing = _BGMData.audioDatas.Find(d => d.AudioClip == _defaultBGMSource.clip);
            if ((_defaultBGMSource is null || data.Value.AudioClip is null).CheckLog("BGM�̍Đ��Ŗ�肪�������܂���")) return;

            switch (mode)
            {
                case BGMMode.CrossFade:
                    _fadeCoroutine = StartCoroutine(FadeBGM(data, playing, true));
                    break;
                case BGMMode.FadeOut:
                    _fadeCoroutine = StartCoroutine(FadeBGM(data, playing, false));
                    break;
                case BGMMode.Stop:
                    _defaultBGMSource.Stop();
                    break;
                case BGMMode.Default:
                    if (_defaultBGMSource.clip != data.Value.AudioClip)
                    {
                        _defaultBGMSource.Stop();
                        _defaultBGMSource.volume = data.Value.AudioVolume;
                        _defaultBGMSource.clip = data.Value.AudioClip;
                        _defaultBGMSource.Play();
                    }
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
            if ((_defaultBGMSource is null || data.Value.AudioClip is null).CheckLog("BGM�̍Đ��Ŗ�肪�������܂���")) return;

            _defaultBGMSource.Stop();
            _defaultBGMSource.volume = data.Value.AudioVolume;
            _defaultBGMSource.clip = data.Value.AudioClip;
            _defaultBGMSource.Play();
        }
        private IEnumerator FadeBGM(AudioData? data, AudioData? playingData, bool ChangeBGM)
        {
            float playingBGMVol = _defaultBGMSource.volume;
            if (!ChangeBGM)
            {
                //�t�F�[�h�A�E�g�݂̂̏���
                for (float time = 0; time < _changeBGMFadeTime; time += Time.deltaTime)
                {
                    _defaultBGMSource.volume = Mathf.Lerp(playingBGMVol, 0, time / _changeBGMFadeTime);
                    yield return null;
                }
                _defaultBGMSource.Stop();
            }
            else
            {
                if ((_fadeBGMSource is null).CheckLog("fadeBGMSource is null")) yield break;
                //�t�F�[�h�C��������AudioSource��������
                _fadeBGMSource.clip = data.Value.AudioClip;
                _fadeBGMSource.volume = 0;
                _fadeBGMSource.time = _defaultBGMSource.time;
                _fadeBGMSource.Play();

                //�N���X�t�F�[�h����
                for (float time = 0; time < _changeBGMFadeTime; time += Time.deltaTime)
                {
                    _defaultBGMSource.volume = Mathf.Lerp(playingBGMVol, 0, time / _changeBGMFadeTime);
                    _fadeBGMSource.volume = Mathf.Lerp(0, data.Value.AudioVolume, time / _changeBGMFadeTime);
                    yield return null;
                }

                //�f�t�H���g�ƃt�F�[�h�p��BGMSource�����ւ���
                _defaultBGMSource.clip = _fadeBGMSource.clip;
                _defaultBGMSource.volume = _fadeBGMSource.volume;
                _defaultBGMSource.time = _fadeBGMSource.time;
                _defaultBGMSource.Play();
                _fadeBGMSource.Stop();

            }
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
        Default,
        Stop,
        CrossFade,
        FadeOut,
    }

    [Serializable]
    public struct AudioData
    {
        public AudioClip AudioClip;
        [Range(0, 1)]
        public float AudioVolume;
    }
}