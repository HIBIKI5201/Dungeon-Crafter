using DCFrameWork.MainSystem;

namespace DCFrameWork.SceneSystem
{
    public class InGameSystem : SceneSystem_B
    {
        CameraManager _cameraManager;

        protected override void Initialize_S()
        {
            _cameraManager = FindAnyObjectByType<CameraManager>();
            (_cameraManager is null).CheckLog("カメラマネージャーが見つかりません");
        }

        protected override void Think(InputContext input)
        {
            _cameraManager?.CameraMove(input.MoveInput, (byte)input.RotateInput);
        }
    }
}