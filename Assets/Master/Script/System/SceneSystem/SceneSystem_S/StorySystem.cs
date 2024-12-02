using DCFrameWork.MainSystem;
using UnityEngine;

namespace DCFrameWork.SceneSystem
{
    public class StorySystem : SceneSystem_B
    {
        private StoryManager _storyManager;
        private SceneKind _sceneKind;
        protected override void Initialize_S()
        {
            _storyManager = GetComponent<StoryManager>();
            _storyManager?.Initialize();
        }

        protected override void Think(InputContext input)
        {

        }

        [ContextMenu("NextText")]
        public void NextStory() => _storyManager?.NextText();

        public void SetStorySceneData(StoryLoadData data)
        {
            Debug.Log(data.sceneKind);
            Debug.Log(data.StoryData);
            _storyManager?.SetStoryData(data.StoryData);
            _sceneKind = data.sceneKind;
        }

        public void EndStory()
        {
            GameBaseSystem.mainSystem.LoadScene(_sceneKind);
        }
    }

    [System.Serializable]
    public struct StoryLoadData
    {
        public StoryData StoryData;
        public SceneKind sceneKind;
    }
}
