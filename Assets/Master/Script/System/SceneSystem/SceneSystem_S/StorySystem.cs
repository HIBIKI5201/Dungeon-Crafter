using UnityEngine;

namespace DCFrameWork.SceneSystem
{
    public class StorySystem : SceneSystem_B
    {
        private StoryManager _storyManager;


        [SerializeField]
        private StoryData _storyData;

        protected override void Initialize_S()
        {
            _storyManager = new StoryManager();
            _storyManager.SetStoryData(_storyData);
        }

        protected override void Think(InputContext input)
        {

        }

        [ContextMenu("NextText")]
        public void NextStory() => _storyManager.NextText();

        public void SetStoryData(StoryData data) => _storyManager.SetStoryData(data);
    }
}
