using UnityEngine;
using UnityEngine.UIElements;
using System.Threading.Tasks;
using UnityEngine.PlayerLoop;
using System;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace DCFrameWork
{
    public partial class TitleWindow : VisualElement
    {
        public Task InitializeTask { get; private set; }
        TitleWindow() => InitializeTask = Initialize();
        private async Task Initialize()
        {
            AsyncOperationHandle<VisualTreeAsset> handle = Addressables.LoadAssetAsync<VisualTreeAsset>("UXML/TitleWindow.uxml");
            await handle.Task;
        }
    }
}
