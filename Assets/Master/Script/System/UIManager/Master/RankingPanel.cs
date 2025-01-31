using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UIElements;

namespace DCFrameWork
{
    [UxmlElement]
    public partial class RankingPanel : VisualElement_B
    {
        public Task InitializeTask { get; private set; }
        
        Label _firstNameText;
        Label _secondNameText;
        Label _thirdNameText;
        Label _firstScoreText;
        Label _secondScoreText;
        Label _thirdScoreText;
        public string FirstRankingText{get=>_firstNameText.text;set=>_firstNameText.text=value;}
        public string SecondRankingText {get => _secondNameText.text;set=> _secondNameText.text = value;}
        public string ThirdRankingText {get=> _thirdNameText.text;set=> _thirdNameText.text = value;}
        public string FirstScoreText {get=> _firstScoreText.text;set=> _firstScoreText.text = value;}
        public string SecondScoreText {get=> _secondScoreText.text;set=> _secondScoreText.text = value;}
        public string ThirdScoreText {get=> _thirdScoreText.text;set=> _thirdScoreText.text = value;}
        public RankingPanel() : base("UXML/RankingPanel.uxml") { }

        protected override async Task Initialize_S(TemplateContainer container){
            _firstNameText = container.Q<Label>("FirstNameText");
            _secondNameText = container.Q<Label>("SecondNameText");
            _thirdNameText = container.Q<Label>("ThirdNameText");
            _firstScoreText = container.Q<Label>("FirstScoreText");
            _secondScoreText = container.Q<Label>("SecondScoreText");
            _thirdScoreText = container.Q<Label>("ThirdScoreText");
        }
    }
}
