using DCFrameWork.InputBuffer;
using DCFrameWork.MainSystem;

namespace DCFrameWork.SceneSystem
{
    public class InGameSystem : SceneSystem_B
    {
        CameraManager _cameraManager;

        protected override void Init_S()
        {
            _cameraManager = FindAnyObjectByType<CameraManager>();
            (_cameraManager is null).CheckLog("カメラマネージャーが見つかりません");
        }

        protected override void Think(InputContext input)
        {
            _cameraManager?.CameraMove(input.MoveInput);
        }
    }
}