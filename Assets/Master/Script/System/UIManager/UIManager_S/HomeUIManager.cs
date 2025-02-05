using DCFrameWork.MainSystem;
using DCFrameWork.SceneSystem;
using System;
using System.Threading.Tasks;
using UnityEngine.UIElements;

namespace DCFrameWork.UI
{
    public class HomeUIManager : UIManager_B
    {
        private TitleWindow _titleWindow;
        private HomeWindow _homeWindow;


        protected override async Task LoadDocumentElement(VisualElement root)
        {
            await Task.CompletedTask;
            _titleWindow  = root.Q<TitleWindow>();
            _homeWindow = root.Q<HomeWindow>();
            _titleWindow.GameStartButton.clicked += () => _titleWindow.style.visibility = Visibility.Hidden;
            //_homeWindow.TitleButton.clicked += () => _titleWindow.style.visibility = Visibility.Visible;
            _homeWindow.StageButton.clicked += () => GameBaseSystem.mainSystem.LoadScene(SceneKind.Ingame_1);
            //_startButton.clicked += () => GameBaseSystem.mainSystem.LoadScene(SceneKind.Ingame_1);
        }
    }
}
