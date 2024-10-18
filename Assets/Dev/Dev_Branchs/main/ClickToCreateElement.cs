using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class ClickToCreateElement : MonoBehaviour
{
    private VisualElement uiContainer;
    private VisualElement element;

    private Coroutine coroutine;

    void Start()
    {
        var uiDocument = GetComponent<UIDocument>();
        uiContainer = uiDocument.rootVisualElement.Q<VisualElement>("uiContainer");
        element = uiContainer.Q<VisualElement>("element");

        uiContainer.RegisterCallback<MouseDownEvent>(OnMouseDown);
    }

    IEnumerator enumerator()
    {
        float timer = 0;
        element.transform.scale = Vector3.zero;
        while (timer < 3)
        {
            timer += Time.deltaTime;
            element.transform.scale += new Vector3(1, 1, 1) * Time.deltaTime;
            yield return null;
        }
    }

    private void OnMouseDown(MouseDownEvent evt)
    {
        Vector2 clickPosition = evt.mousePosition;
        float width = 100;
        float height = 100;
        if (element is null)
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
        else
        {
            element.transform.scale = Vector3.one;
        }

        element.style.left = clickPosition.x - width / 2;
        element.style.top = clickPosition.y - height / 2;

        if (coroutine != null) StopCoroutine(coroutine);
        coroutine = StartCoroutine(enumerator());
    }
}
