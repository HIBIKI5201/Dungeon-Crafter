using DCFrameWork.MainSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DCFrameWork.SceneSystem
{
    public class StoryManager : MonoBehaviour
    {
        private IEnumerator _enumerator;
        private List<StoryText> _storyData;

        private StoryUIManager _storyUIManager;
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
                Debug.Log($"{_storyData[count]._character} {_storyData[count]._animation}\n{_storyData[count]._text}");
                _storyUIManager.TextBoxUpdate(_storyData[count]._character, _storyData[count]._text);
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
    }
}
