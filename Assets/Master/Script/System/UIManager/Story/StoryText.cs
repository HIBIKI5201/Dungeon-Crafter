using System;
using System.Threading.Tasks;
using UnityEngine.UIElements;

namespace DCFrameWork
{
    [UxmlElement]
    public partial class StoryText : VisualElement_B
    {
        Label _name;
        Label _text;
        VisualElement _textBox;
        public string Name { set { _name.text = value; } }
        public string Text { set { _text.text = value; } }
        public Action TextBoxClickEvent { set { _textBox.RegisterCallback<ClickEvent>(x => value()); } }
        public StoryText() : base("UXML/Story/StoryText") { }

        protected override Task Initialize_S(TemplateContainer container)
        {
            _name = container.Q<Label>("Name");
            _text = container.Q<Label>("Text");
            _name.enableRichText = true;
            _text.enableRichText = true;
            _textBox = container.Q<VisualElement>("TextBox");

            return Task.CompletedTask;
        }
    }
}
