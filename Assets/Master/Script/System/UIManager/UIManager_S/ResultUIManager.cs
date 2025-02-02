using DCFrameWork.MainSystem;
using DCFrameWork.UI;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace DCFrameWork
{
    public class ResultUIManager : UIManager_B
    {
        VisualElement _ranking;
        VisualElement _resultPanel;

        VisualElement _rankingTitleButton;
        VisualElement _rankingOption;

        Button _titleButton;
        Button _rankingButton;
        protected override async Task LoadDocumentElement(VisualElement root)
        {
            _titleButton = root.Q<Button>("TitleButton");
            _rankingButton = root.Q<Button>("RankingButton");
            _resultPanel = root.Q<VisualElement>("ResultPanel");
            _rankingTitleButton = root.Q<VisualElement>("ReturnButton");
            _rankingOption = root.Q<VisualElement>("SettingButton");
            _ranking = root.Q<VisualElement>("Ranking");
            _resultPanel.style.display = DisplayStyle.Flex;
            _ranking.style.display = DisplayStyle.None;

            _titleButton.clicked += () => GameBaseSystem.mainSystem.LoadScene(SceneKind.Home);
            _rankingButton.clicked += () =>
            {
                _resultPanel.style.display = DisplayStyle.None;
                _ranking.style.display = DisplayStyle.Flex;
            };
            _rankingOption.AddManipulator(new Clickable(() => Debug.Log("Option clicked")));
            _rankingTitleButton.AddManipulator(new Clickable(() =>
            {
                _resultPanel.style.display = DisplayStyle.Flex;
                _ranking.style.display = DisplayStyle.None;
            }));
        }
        private void OnDisable()
        {
            _titleButton.clicked -= () => GameBaseSystem.mainSystem.LoadScene(SceneKind.Home);
            _rankingOption.RemoveManipulator(new Clickable(() => Debug.Log("Option clicked")));
            _rankingTitleButton.RemoveManipulator(new Clickable(() =>
            {
                _resultPanel.style.display = DisplayStyle.Flex;
                _ranking.style.display = DisplayStyle.None;
            }));
            _rankingButton.clicked -= () =>
            {
                _resultPanel.style.display = DisplayStyle.None;
                _ranking.style.display = DisplayStyle.Flex;
            };
        }
    }
}
