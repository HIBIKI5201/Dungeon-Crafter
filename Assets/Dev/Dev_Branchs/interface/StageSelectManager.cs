using DCFrameWork.MainSystem;
using DCFrameWork.SceneSystem;
using UnityEngine;

public class StageSelectManager : MonoBehaviour
{
    [SerializeField] StageSelectManagerData Data;
    public void Stage()
    {
        GameBaseSystem.mainSystem.LoadScene<StorySystem>(SceneKind.Story, x => x.SetStorySceneData(Data));
    }
}
[System.Serializable]
public struct StageSelectManagerData
{
    public StoryData firstStoryData;
    public StoryData afterStoryData;
    public SceneKind sceneKind;
}