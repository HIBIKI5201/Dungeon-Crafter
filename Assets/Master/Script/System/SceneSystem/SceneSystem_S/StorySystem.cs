using DCFrameWork.MainSystem;
using UnityEngine;

namespace DCFrameWork.SceneSystem
{
    public class StorySystem : SceneSystem_B
    {
        private StoryManager _storyManager;
        private SceneKind _sceneKind;
        private StoryData _endStoryData;
        protected override void Initialize_S()
        {
            _storyManager = new StoryManager();
        }

        protected override void Think(InputContext input)
        {

        }

        [ContextMenu("NextText")]
        public void NextStory() => _storyManager.NextText();

        public void SetStorySceneData(StageSelectManagerData data)
        {
            _storyManager.SetStoryData(data.firstStoryData);
            _sceneKind = data.sceneKind;
            _endStoryData = data.afterStoryData;
        }

        public void EndStory()
        {
            GameBaseSystem.mainSystem.LoadScene<InGameSystem>(_sceneKind, system => system.SetStoryData(_endStoryData));
        }
    }
}
