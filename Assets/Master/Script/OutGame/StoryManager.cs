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
        public void Initialize()
        {
            _enumerator = PlayStoryContext();
        }

        public void SetStoryData(StoryData storyText)
        {
            _storyData = storyText.StoryText;
        }

        public void NextContext() => _enumerator.MoveNext();

        private IEnumerator PlayStoryContext()
        {
            yield return null;
        }
    }
}
