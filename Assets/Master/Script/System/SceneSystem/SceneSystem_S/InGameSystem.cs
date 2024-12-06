using UnityEngine;
using DCFrameWork.MainSystem;

namespace DCFrameWork.SceneSystem
{
    public class InGameSystem : SceneSystem_B
    {
        private CameraManager _cameraManager;

        private LevelManager _levelManager;
        [SerializeField]
        private StoryData _storyData;

        protected override void Initialize_S()
        {
            _cameraManager = FindAnyObjectByType<CameraManager>();
            (_cameraManager is null).CheckLog("カメラマネージャーが見つかりません");
            _levelManager = FindAnyObjectByType<LevelManager>();
            (_levelManager is null).CheckLog("レベルマネージャーが見つかりません");
        }

        protected override void Think(InputContext input)
        {
            _cameraManager?.CameraMove(input.MoveInput, input.RotateInput);
        }

        public void SetStoryData(StoryData storyData)
        {
            _storyData = storyData;
        }

        public void FailEndGame() => GameBaseSystem.mainSystem.LoadScene(SceneKind.Home);
        public void SuccessEndGame() => GameBaseSystem.mainSystem.LoadScene<StorySystem>(SceneKind.Story, system => system.SetStorySceneData(new StoryLoadData { StoryData = _storyData, sceneKind = SceneKind.Home }));
    }
}