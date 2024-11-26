namespace DCFrameWork.SceneSystem
{
    public class StorySystem : SceneSystem_B
    {
        private StoryManager _storyManager;

        protected override void Initialize_S()
        {
            _storyManager = GetComponentInChildren<StoryManager>();
            _storyManager.Initialize();
        }

        protected override void Think(InputContext input)
        {

        }

        public void NextStory() => _storyManager.NextContext();

        public void SetStoryData(StoryData data) => _storyManager.SetStoryData(data);
    }
}
