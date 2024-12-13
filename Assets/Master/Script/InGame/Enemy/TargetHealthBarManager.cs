using UnityEngine;
using UnityEngine.UI;

namespace DCFrameWork
{
    public class TargetHealthBarManager : MonoBehaviour
    {
        private Image _image;
        private void Start()
        {
            _image = transform.GetChild(0).GetComponent<Image>();
        }
        public void BarFillUpdate(float value) => _image.fillAmount = value;

        public void FollowTarget(Transform target)
        {

            Vector3 cameraPos = Camera.main.transform.position;
            Vector3 towards = target.position + new Vector3(target.position.x - cameraPos.x, 0, target.position.z - cameraPos.z).normalized;
            Vector2 screenPos = Camera.main.WorldToScreenPoint(towards);
            transform.position = screenPos;
        }
    }
}
