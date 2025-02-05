using System.Threading.Tasks;
using UnityEngine.UIElements;

namespace DCFrameWork
{
    [UxmlElement]
    public partial class HomeWindow : VisualElement_B
    {
        //UI要素
        private Button _stageButton;
        public Button StageButton { get => _stageButton; }
        private Button _rankingButton;
        private Button _storyButton;
        //private Button _titleButton;タイトルボタンはbuttonとして作られていなかったためタイトル画面に遷移しなおすのは諦め
        //public Button TitleButton { get => _titleButton; }
        private Button _settingButton;
        private Label _outGameinfo;
        public HomeWindow() : base("UXML/Home/Menu") { }

        protected override Task Initialize_S(TemplateContainer container)
        {
            //UI要素取得
            _stageButton = container.Q<Button>("StageButton");
            _rankingButton = container.Q<Button>("RankingButton");
            _storyButton = container.Q<Button>("StoryButton");
            //_titleButton = container.Q<Button>("TitleButton");
            _settingButton = container.Q<Button>("SettingButton");
            _outGameinfo = container.Q<Label>("OutGameOInfo");

            return Task.CompletedTask;
        }
    }
}
