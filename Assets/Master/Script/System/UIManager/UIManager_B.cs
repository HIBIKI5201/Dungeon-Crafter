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
                Debug.LogWarning("UIDocument���A�^�b�`����Ă��܂���");
                return;
            }
            _root = _document?.rootVisualElement;

            await LoadDocumentElement(_root);
        }

        /// <summary>
        /// UIDocument�̗v�f�����[�h���郁�\�b�h
        /// Start�Ŏ��s�����
        /// </summary>
        /// <param name="root">Document��root�v�f</param>
        protected abstract Task LoadDocumentElement(VisualElement root);
    }
}