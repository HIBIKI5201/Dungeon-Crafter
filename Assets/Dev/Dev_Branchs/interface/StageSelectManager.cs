using DCFrameWork.MainSystem;
using DCFrameWork.SceneSystem;
using UnityEngine;

public class StageSelectManager : MonoBehaviour
{
    [SerializeField] StorySceneData Data;
    public void Stage()
    {
        GameBaseSystem.mainSystem.LoadScene<StorySystem>(SceneKind.Story, system => system.SetStorySceneData(Data));
    }
}