using DCFrameWork.MainSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DCFrameWork.SceneSystem
{
    public class StoryManager : MonoBehaviour
    {
        private AudioSource _audioSource;

        private IEnumerator _enumerator;
        private List<StoryText> _storyData;

        private StoryUIManager _storyUIManager;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void Initialize(StoryUIManager storyUIManager)
        {
            _enumerator = PlayStoryContext();
            _storyUIManager = storyUIManager;
        }

        public void SetStoryData(StoryData storyText) => _storyData = storyText.StoryText;

        public void NextText() => _enumerator.MoveNext();

        private IEnumerator PlayStoryContext()
        {
            int count = 0;
            while (count < _storyData.Count)
            {
                StoryText storyText = _storyData[count];

                Debug.Log($"{storyText.Character} {storyText.Animation}\n{storyText.Text}");
                _storyUIManager.TextBoxUpdate(storyText.Character, storyText.Text);
                _audioSource.PlayOneShot(storyText.AudioClip);
                count++;
                yield return null;
            }
            EndStory();
        }

        private void EndStory()
        {
            var system = GameBaseSystem.sceneSystem as StorySystem;
            system.EndStory();
        }

        public bool SetAudioVolume(float volume)
        {
            if (volume < 0 || 1 < volume) {
                return false;
            }

            _audioSource.volume = volume;
            return true;
        }
    }
}
