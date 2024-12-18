using DCFrameWork.MainSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace DCFrameWork.Enemy
{
    public class EnemyHealthBarManager : MonoBehaviour
    {
        private Image _thisImage;
        private Image _image;
        public void Initialize()
        {
            _thisImage = GetComponent<Image>();
            _image = transform.GetChild(0).GetComponent<Image>();
            _thisImage.enabled = false;
            _image.enabled = false;
        }
        private void Start()
        {
            StartCoroutine(WaitRendering());
        }

        IEnumerator WaitRendering()
        {
            yield return FrameWork.PausableWaitForSecond(0.1f);
            _thisImage.enabled = true;
            _image.enabled = true;
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
