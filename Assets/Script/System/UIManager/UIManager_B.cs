using UnityEngine;
using UnityEngine.UIElements;

public abstract class UIManager_B : MonoBehaviour
{
    protected UIDocument _document;
    protected VisualElement _root;

    private void Start()
    {
        _document = GetComponent<UIDocument>();
        if (_document is null)
        {
            Debug.LogWarning("UIDocument���A�^�b�`����Ă��܂���");
            return;
        }
        _root = _document?.rootVisualElement;

        LoadDocumentElement(_root);
    }

    /// <summary>
    /// ������
    /// </summary>
    public abstract void Init();

    /// <summary>
    /// UIDocument�̗v�f�����[�h���郁�\�b�h
    /// Start�Ŏ��s�����
    /// </summary>
    /// <param name="root">Document��root�v�f</param>
    protected abstract void LoadDocumentElement(VisualElement root);
}
