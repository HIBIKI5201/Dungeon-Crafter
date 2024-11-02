using UnityEngine;
using UnityEngine.VFX;

namespace DCFrameWork
{
    public class ClickDetection : MonoBehaviour
    {
        private VisualEffect _effect;
        private void Start()
        {
            _effect = GetComponent<VisualEffect>();
        }
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _effect.SetVector3("MousePosition", Input.mousePosition);
                _effect.SendEvent("OnClick");
            }
        }
    }
}
