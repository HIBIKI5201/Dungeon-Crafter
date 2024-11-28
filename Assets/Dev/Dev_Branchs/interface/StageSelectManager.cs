using DCFrameWork.MainSystem;
using DCFrameWork.SceneSystem;

public class StageSelectManager
{
    private static StageSelectManager _instance;
    public static StageSelectManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new StageSelectManager();
            }
            return _instance;
        }
    }
    private StageSelectManager() { }
    public void Stage1()
    {
        GameBaseSystem.mainSystem.LoadScene(SceneKind.Ingame_1);
    }

    public void Story(StoryData storyData)
    {
        GameBaseSystem.mainSystem.LoadScene<StorySystem>(SceneKind.Story, x => x.SetStoryData(storyData));
    }
}