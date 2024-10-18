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
        float width = 100;
        float height = 100;
        if (element is not null)
        {
            width = element.resolvedStyle.width;
            height = element.resolvedStyle.height;
        }
        else
        {
            element = new VisualElement
            {
                style =
                {
                    width = width,
                    height = height,
                    backgroundColor = new Color(Random.value, Random.value, Random.value, 1)
                }
            };

            uiContainer.Add(element);

            element.style.position = Position.Absolute;
        }


        element.MarkDirtyRepaint();


        element.style.left = clickPosition.x - width / 2;
        element.style.top = clickPosition.y - height / 2;
    }
}
