using UnityEngine;
using UnityEngine.UIElements;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Linq;
using System.Collections.Generic;

namespace DCFrameWork
{
    [UxmlElement]
    public partial class OutGameUIBase : VisualElement
    {
        private string _addressablesPass;
        [UxmlAttribute]
        public string AddressablesPass { get => _addressablesPass; set => _addressablesPass = value; }
        public Dictionary<string, VisualElement> OutGameUIElements { get; private set; }
        public Task InitializeTask { get; private set; }

        public OutGameUIBase()
        {
            OutGameUIElements = new Dictionary<string, VisualElement>();
            InitializeTask = Initialize();
        }

        private async Task Initialize()
        {
            if (string.IsNullOrEmpty(_addressablesPass))
            {
                Debug.LogError("AddressablesPass is null or empty.");
                return;
            }

            AsyncOperationHandle<VisualTreeAsset> handle = Addressables.LoadAssetAsync<VisualTreeAsset>(_addressablesPass);
            await handle.Task;

            if (handle.Status == AsyncOperationStatus.Succeeded && handle.Result != null)
            {
                var treeAsset = handle.Result;
                var container = treeAsset.Instantiate();
                container.style.width = Length.Percent(100);
                container.style.height = Length.Percent(100);
                VisualElement[] visualElements = container.Children().ToArray();
                foreach (VisualElement visualElement in visualElements)
                {
                    OutGameUIElements.Add(visualElement.name, visualElement);
                }
            }
            else
            {
                Debug.LogError($"Failed to load UXML file from Addressables: {_addressablesPass}");
            }

            Addressables.Release(handle);
        }
    }
}
