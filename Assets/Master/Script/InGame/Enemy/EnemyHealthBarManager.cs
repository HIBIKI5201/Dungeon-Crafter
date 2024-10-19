using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarManager : MonoBehaviour
{
    RectTransform _rectTransform;
    [SerializeField]
    Image _image;

    [SerializeField]
    CapsuleCollider _targetCollider;

  
    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public void FollowTarget(Vector3 target)
    {
        
        target.y -= _targetCollider.height / 2;
        Vector2 screenPos = Camera.main.WorldToScreenPoint(target);
        transform.position = screenPos;
    }

    public void BarFillUpdate(float value) => _image.fillAmount = value;
}
