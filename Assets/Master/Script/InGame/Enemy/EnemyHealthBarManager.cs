using UnityEngine;
using UnityEngine.UI;


namespace DCFrameWork.Enemy
{
    public class EnemyHealthBarManager : MonoBehaviour
    {
        [SerializeField]
        Image _image ;

       
        public void BarFillUpdate(float value) => _image.fillAmount = value;

        
    }
}
