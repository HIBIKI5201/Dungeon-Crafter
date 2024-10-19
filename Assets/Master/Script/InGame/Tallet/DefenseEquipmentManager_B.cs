using UnityEngine;

namespace DCFrameWork.DefenseEquipment
{
    public abstract class DefenseEquipmentManager_B : MonoBehaviour
    {
        [SerializeField]
        protected DefenseEquipmentData_B _data;

        protected int Level;

        private void Start()
        {
            if (_data is null)
                Debug.Log("�f�[�^������܂���");
        }

        private void Update()
        {
            Think();
        }

        protected abstract void Think();
    }
}