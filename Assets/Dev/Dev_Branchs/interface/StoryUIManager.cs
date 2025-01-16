using UnityEngine;
using DCFrameWork.UI;
using System.Threading.Tasks;
using UnityEngine.UIElements;
using DCFrameWork.MainSystem;
using DCFrameWork.SceneSystem;
using System;
namespace DCFrameWork
{
    public class StoryUIManager : UIManager_B
    {
        StoryText _storyText;
        protected override async Task LoadDocumentElement(VisualElement root)
        {
            _storyText = root.Q<StoryText>("StoryText");
            await _storyText.InitializeTask;
            _storyText.TextBoxClickEvent = ()=>(GameBaseSystem.sceneSystem as StorySystem).NextStory();
        }
        public void TextBoxUpdate(string name,string text)
        {
            //名前が「システム」の時だけ空文字にする
            _storyText.Name = (name != "システム") ? name : string.Empty;
            _storyText.Text = text;
        }
    }
}
