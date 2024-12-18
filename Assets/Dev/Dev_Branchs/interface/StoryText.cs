using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.UIElements;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System;
using Unity.AppUI.UI;

namespace DCFrameWork
{
    [UxmlElement]
    public partial class StoryText : VisualElement
    {
        public Task InitializeTask { get; private set; }
        Label _name;
        Label _text;
        VisualElement _textBox;
        public string Name { set { _name.text = value; } }
        public string Text { set { _text.text = value; } }
        public Action TextBoxClickEvent { set { _textBox.RegisterCallback<ClickEvent>(x => value()); } }
        public StoryText() => InitializeTask = Initialize();
        private async Task Initialize()
        {
            AsyncOperationHandle<VisualTreeAsset> handle = Addressables.LoadAssetAsync<VisualTreeAsset>("UXML/StoryText.uxml");
            await handle.Task;
            if (handle.Status == AsyncOperationStatus.Succeeded && handle.Result != null)
            {
                var treeAsset = handle.Result;
                var container = treeAsset.Instantiate();
                container.style.width = Length.Percent(100);
                container.style.height = Length.Percent(100);
                this.RegisterCallback<KeyDownEvent>(e => e.StopImmediatePropagation());
                pickingMode = PickingMode.Ignore;
                container.RegisterCallback<KeyDownEvent>(e => e.StopImmediatePropagation());
                container.pickingMode = PickingMode.Ignore;
                hierarchy.Add(container);
                _name = container.Q<Label>("Name");
                _text = container.Q<Label>("Text");
                _name.enableRichText = true;
                _text.enableRichText = true;
                _textBox = container.Q<VisualElement>("TextBox");
                Debug.Log("ウィンドウは正常にロード完了");
            }
            else
            {
                Debug.LogError("Failed to load UXML file from Addressables: UXML/StoryText.uxml");
            }
            Addressables.Release(handle);
        }
    }
}
