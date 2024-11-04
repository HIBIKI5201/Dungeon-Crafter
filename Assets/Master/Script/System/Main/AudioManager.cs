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
        private AudioSource _voiceSource;

        [SerializeField]
        List<AudioData> _inGameSoundEffectList = new();
        [SerializeField]
        List<AudioData> _outGameSoundEffectList = new();
        [SerializeField]
        List<AudioData> _BGMList = new();
        [SerializeField]
        List<AudioData> _voiceList = new();

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
                    PlaySoundEffect(GetAudioData(_inGameSoundEffectList, index));
                    break;
                case SoundKind.OutGame:
                    PlaySoundEffect(GetAudioData(_outGameSoundEffectList, index));
                    break;
                case SoundKind.BGM:
                    PlayBGM(GetAudioData(_BGMList, index));
                    break;
                    case SoundKind.Voice:
                    PlayBGM(GetAudioData(_voiceList, index));
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

    [Serializable]
    public struct AudioData
    {
        public AudioClip AudioClip;
        [Range(0, 1)]
        public float AudioVolume;
    }
}