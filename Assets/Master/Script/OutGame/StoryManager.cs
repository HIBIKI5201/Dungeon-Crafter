using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DCFrameWork.SceneSystem
{
    public class StoryManager
    {
        private IEnumerator _enumerator;
        private List<StoryText> _storyData;
        public StoryManager()
        {
            _enumerator = PlayStoryContext();
        }

        public void SetStoryData(StoryData storyText) => _storyData = storyText.StoryText;

        public void NextText() => _enumerator.MoveNext();

        private IEnumerator PlayStoryContext()
        {
            int count = 0;
            while (count < _storyData.Count)
            {
                Debug.Log($"{_storyData[count]._character} {_storyData[count]._animation}\n{_storyData[count]._text}");

                count++;
                yield return null;
            }
            EndStory();
        }

        private void EndStory()
        {
            Debug.Log("End Story");
        }
    }
}
