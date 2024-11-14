namespace DCFrameWork.SceneSystem
{
    public class HomeSystem : SceneSystem_B
    {


        protected override void Initialize_S()
        {
        }

        protected override void Think(InputContext input)
        {
        }
    }

    public enum HomeWindowState
    {
        Title,
        Story,
        MainMenu,
        StageSelect,
    }
}