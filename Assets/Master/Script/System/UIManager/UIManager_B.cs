using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace DCFrameWork.UI
{
    public abstract class UIManager_B : MonoBehaviour
    {
        protected UIDocument _document;
        protected VisualElement _root;

        public async Task Initialize()
        {
            _document = GetComponent<UIDocument>();
            if (_document is null)
            {
                Debug.LogWarning("UIDocumentがアタッチされていません");
                return;
            }
            _root = _document?.rootVisualElement;

            await LoadDocumentElement(_root);
        }

        /// <summary>
        /// UIDocumentの要素をロードするメソッド
        /// Startで実行される
        /// </summary>
        /// <param name="root">Documentのroot要素</param>
        protected abstract Task LoadDocumentElement(VisualElement root);
    }
}