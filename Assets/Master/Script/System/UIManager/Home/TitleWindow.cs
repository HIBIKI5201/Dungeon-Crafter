using System;
using System.Threading.Tasks;
using UnityEngine.UIElements;

namespace DCFrameWork
{
    [UxmlElement]
    public partial class TitleWindow : VisualElement_B
    {
        //UI—v‘f
        private Button _gameStartButton;
        private Button _creditButton;
        public Button GameStartButton { get => _gameStartButton; }
        public TitleWindow() : base("UXML/Home/TitleWindow") { }
        protected override Task Initialize_S(TemplateContainer container)
        {
            //UI—v‘fŽæ“¾
            _gameStartButton = container.Q<Button>("GameStartButton");
            _creditButton = container.Q<Button>("CreditButton");

            return Task.CompletedTask;
        }
    }
}
