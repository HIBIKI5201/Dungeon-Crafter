using DCFrameWork.InputBuffer;
using UnityEngine;

public class HomeSystem : SceneSystem_B<HomeInputBuffer, UIManager_B>
{
    CameraManager _cameraManager;

    protected override void Init_S()
    {
        _cameraManager = FindAnyObjectByType<CameraManager>();
    }

    protected override void Think(InputContext input)
    {
        _cameraManager.CameraMove(input.MoveInput);
    }

    private enum WindowKind
    {
        Title,
        Story,
        MainManu,
        StageSelect,
    }
}
