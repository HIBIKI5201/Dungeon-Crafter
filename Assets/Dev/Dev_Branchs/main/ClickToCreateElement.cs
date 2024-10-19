using UnityEngine;
using UnityEngine.UIElements;

public class ClickToCreateElement : MonoBehaviour
{
    private VisualElement uiContainer;
    private VisualElement element;

    public GameObject particle;
    private ParticleSystem particleSystem;

    void Start()
    {
        var uiDocument = GetComponent<UIDocument>();
        uiContainer = uiDocument.rootVisualElement.Q<VisualElement>("uiContainer");
        element = uiContainer.Q<VisualElement>("element");

        uiContainer.RegisterCallback<MouseDownEvent>(OnMouseDown);

        particleSystem = particle.GetComponent<ParticleSystem>();
    }

    private void OnMouseDown(MouseDownEvent evt)
    {
        if (element is null) return;

        Vector2 clickPosition = evt.mousePosition;

        float width = element.resolvedStyle.width;
        float height = element.resolvedStyle.height;

        element.style.left = clickPosition.x - width / 2;
        element.style.top = clickPosition.y - height / 2;

        particleSystem?.Clear();
        particleSystem?.Play();
    }
}
