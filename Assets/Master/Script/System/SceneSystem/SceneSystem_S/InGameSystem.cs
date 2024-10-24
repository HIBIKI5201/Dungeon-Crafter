using DCFrameWork.InputBuffer;
using DCFrameWork.MainSystem;
using UnityEngine;

public class InGameSystem : SceneSystem_B<InGameInputBuffer, UIManager_B>
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
