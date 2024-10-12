using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarManager : MonoBehaviour
{
    RectTransform _rectTransform;
    [SerializeField]
    Image _image;
    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public void FollowTarget(Vector3 target)
    {
        Vector2 screenPos = Camera.main.WorldToScreenPoint(target);
        _rectTransform.position = screenPos + new Vector2(0, -200);
    }

    public void BarFillUpdate(float value) => _image.fillAmount = value;
}
