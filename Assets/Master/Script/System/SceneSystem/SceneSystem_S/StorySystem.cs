using DCFrameWork.MainSystem;
using UnityEngine;

namespace DCFrameWork.SceneSystem {
    public class StorySystem : SceneSystem_B {
        private StoryManager _storyManager;
        private SceneKind _sceneKind;

#if UNITY_EDITOR
        [SerializeField]
        private StoryData _debugStoryData;
#endif

        protected override void Initialize_S()
        {
            _storyManager = GetComponent<StoryManager>();
            _storyManager?.Initialize(_UIManager as StoryUIManager);
        }

        protected override void Think(InputContext input)
        {

        }

        [ContextMenu("NextText")]
        public void NextStory() => _storyManager?.NextText();

        public async void SetStorySceneData(StoryLoadData data)
        {
            _storyManager?.SetStoryData(data.StoryData);
            _sceneKind = data.sceneKind;
            await Awaitable.WaitForSecondsAsync(1);
            NextStory();
        }

        public void EndStory()
        {
            GameBaseSystem.mainSystem.LoadScene(_sceneKind);
        }

#if UNITY_EDITOR
        [ContextMenu("DebugStory")]
        public void DebugStory()
        {
            SetStorySceneData(new StoryLoadData() { StoryData = _debugStoryData, sceneKind = SceneKind.Home});
        }
#endif
    }

    [System.Serializable]
    public struct StoryLoadData {
        public StoryData StoryData;
        public SceneKind sceneKind;
    }
}
