using UnityEngine;
using UnityEngine.UIElements;

public class ClickToCreateElement : MonoBehaviour
{
    private VisualElement uiContainer;
    private VisualElement element;

    public GameObject particle;
    private ParticleSystem _particleSystem;

    void Start()
    {
        var uiDocument = GetComponent<UIDocument>();
        uiContainer = uiDocument.rootVisualElement.Q<VisualElement>("uiContainer");
        element = uiContainer.Q<VisualElement>("element");

        uiContainer.RegisterCallback<MouseDownEvent>(OnMouseDown);

        _particleSystem = particle.GetComponent<ParticleSystem>();
    }

    private void OnMouseDown(MouseDownEvent evt)
    {
        Vector2 clickPosition = evt.mousePosition;

        float width = element.resolvedStyle.width;
        float height = element.resolvedStyle.height;

        element.style.left = clickPosition.x - width / 2;
        element.style.top = clickPosition.y - height / 2;

        _particleSystem?.Clear();
        _particleSystem?.Play();
    }
}
