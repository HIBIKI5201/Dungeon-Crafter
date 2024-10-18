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

        public void PlayBGM(int index)
        {

        }

        public void PlaySoundEffect(int index)
        {

        }
    }
}