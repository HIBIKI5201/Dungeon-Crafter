using UnityEngine;
using UnityEngine.UIElements;

public abstract class UIManager_B : MonoBehaviour
{
    protected UIDocument _document;
    protected VisualElement _root;

    private void Start()
    {
        _document = GetComponent<UIDocument>();
        _root = _document.rootVisualElement;
    }

    protected abstract void Init();
}
