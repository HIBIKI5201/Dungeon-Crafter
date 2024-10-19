using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarManager : MonoBehaviour
{
    RectTransform _rectTransform;
    [SerializeField]
    Image _image;

    [SerializeField]
    CapsuleCollider _targetCollider;

    [SerializeField] int _radius = 1 ;
    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public void FollowTarget(Vector3 target)
    {
        Vector3 cameraPos = Camera.main.transform.position;
        Vector3 towards = target  + new Vector3(target.x - cameraPos.x, 0 , target.z - cameraPos.z).normalized   * _radius;
        Vector2 screenPos = Camera.main.WorldToScreenPoint(towards);
        transform.position = screenPos;
    }

    public void BarFillUpdate(float value) => _image.fillAmount = value;
}
