using DCFrameWork.MainSystem;
using DCFrameWork.SceneSystem;
using System;
using System.Threading.Tasks;
using UnityEngine.UIElements;

namespace DCFrameWork.UI
{
    public class HomeUIManager : UIManager_B
    {
        TemplateContainer _title;
        Button _startButton;
        Button _creditButton;
        protected override async Task LoadDocumentElement(VisualElement root)
        {
            await Task.CompletedTask;
            _title = root.Q<TemplateContainer>("Title");
            _startButton = root.Q<Button>("GameStartButton");
            _creditButton = root.Q<Button>("CreditButton");
            _startButton.clicked += () => GameBaseSystem.mainSystem.LoadScene(SceneKind.Ingame_1);
        }
    }
}
