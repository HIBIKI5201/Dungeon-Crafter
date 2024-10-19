using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class ClickToCreateElement : MonoBehaviour
{
    private VisualElement uiContainer;
    private VisualElement element;

    void Start()
    {
        var uiDocument = GetComponent<UIDocument>();
        uiContainer = uiDocument.rootVisualElement.Q<VisualElement>("uiContainer");
        element = uiContainer.Q<VisualElement>("element");

        uiContainer.RegisterCallback<MouseDownEvent>(OnMouseDown);
    }

    private void OnMouseDown(MouseDownEvent evt)
    {
        Vector2 clickPosition = evt.mousePosition;
        float width = 1;
        float height = 1;
        if (element is null)
        {
            element = new VisualElement
            {
                style =
                {
                    width = width,
                    height = height,
                }
            };

            uiContainer.Add(element);
            element.style.position = Position.Absolute;
        }
        else
        {
            width = element.resolvedStyle.width;
            height = element.resolvedStyle.height;
        }

        element.style.left = clickPosition.x - width / 2;
        element.style.top = clickPosition.y - height / 2;
    }
}
