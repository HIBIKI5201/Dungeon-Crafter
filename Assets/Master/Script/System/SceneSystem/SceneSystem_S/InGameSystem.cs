using DCFrameWork.MainSystem;
using System;
using UnityEngine;

namespace DCFrameWork.SceneSystem
{
    public class InGameSystem : SceneSystem_B
    {
        private CameraManager _cameraManager;
        [SerializeField]
        private StoryData StorySceneData;

        protected override void Initialize_S()
        {
            _cameraManager = FindAnyObjectByType<CameraManager>();
            (_cameraManager is null).CheckLog("カメラマネージャーが見つかりません");
        }

        protected override void Think(InputContext input)
        {
            _cameraManager?.CameraMove(input.MoveInput, input.RotateInput);
        }

        public void EndInGame(bool success)
        {
            if (success)
                GameBaseSystem.mainSystem.LoadScene<StorySystem>(SceneKind.Story, system => system.SetStorySceneData(new StorySceneData { StoryData = StorySceneData, sceneKind = SceneKind.Home }));
            else
                GameBaseSystem.mainSystem.LoadScene(SceneKind.Home);
        }
    }

    [System.Serializable]
    public struct StorySceneData
    {
        public StoryData StoryData;
        public SceneKind sceneKind;
    }
}