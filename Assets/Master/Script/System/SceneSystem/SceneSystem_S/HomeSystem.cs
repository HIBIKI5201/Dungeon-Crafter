using DCFrameWork.InputBuffer;
using UnityEngine;

public class HomeSystem : SceneSystem_B
{


    protected override void Init_S()
    {
    }

    protected override void Think(InputContext input)
    {
    }

    private enum WindowKind
    {
        Title,
        Story,
        MainManu,
        StageSelect,
    }
}
