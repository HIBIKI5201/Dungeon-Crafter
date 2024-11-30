using DCFrameWork.MainSystem;

namespace DCFrameWork.SceneSystem
{
    public class InGameSystem : SceneSystem_B
    {
        private CameraManager _cameraManager;
        private StoryData _storyData;

        protected override void Initialize_S()
        {
            _cameraManager = FindAnyObjectByType<CameraManager>();
            (_cameraManager is null).CheckLog("カメラマネージャーが見つかりません");
        }

        protected override void Think(InputContext input)
        {
            _cameraManager?.CameraMove(input.MoveInput, input.RotateInput);
        }

        public void SetStoryData(StoryData storyData)
        {
            _storyData = storyData;
        }

        public void EndInGame(bool success)
        {
            if (success)
                GameBaseSystem.mainSystem.LoadScene<StorySystem>(SceneKind.Story, system => system.SetStorySceneData(new StageSelectManagerData { firstStoryData = _storyData, sceneKind = SceneKind.Home }));
            else
                GameBaseSystem.mainSystem.LoadScene(SceneKind.Home);
        }
    }
}