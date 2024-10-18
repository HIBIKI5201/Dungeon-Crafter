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
            Debug.LogWarning("UIDocumentがアタッチされていません");
            return;
        }
        _root = _document?.rootVisualElement;

        LoadDocumentElement(_root);
    }

    /// <summary>
    /// 初期化
    /// </summary>
    public abstract void Init();

    /// <summary>
    /// UIDocumentの要素をロードするメソッド
    /// Startで実行される
    /// </summary>
    /// <param name="root">Documentのroot要素</param>
    protected abstract void LoadDocumentElement(VisualElement root);
}
