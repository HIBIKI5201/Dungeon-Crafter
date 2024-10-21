using DCFrameWork.MainSystem;
using UnityEngine;

namespace DCFrameWork.DefenseEquipment
{
    public abstract class DefenseEquipmentManager_B<Data> : MonoBehaviour, IPausable where Data : DefenseEquipmentData_B
    {
        [SerializeField]
        private DefenseEquipmentData_B _data;

        protected int Level;

        #region ���ʃX�e�[�^�X
        protected float _attack;
        protected float _rate;
        protected float _range;
        protected float _critical;
        #endregion

        private void Start()
        {
            if (_data is null)
                Debug.Log("�f�[�^������܂���");
            LoadCommonData();
            MainSystem.MainSystem.mainSystem.AddPausableObject(this as IPausable);
        }

        private void OnDestroy()
        {
            MainSystem.MainSystem.mainSystem.RemovePausableObject(this as IPausable);
        }

        private void LoadCommonData()
        {
            Data data = _data as Data;

            _attack = data.Attack;
            _rate = data.Rate;
            _range = data.Range;
            _critical = data.Critical;

            LoadSpecificData(data);
        }

        /// <summary>
        /// �ݒ肵���^�p�����[�^�ɑΉ�������p�ϐ��������Ă�������
        /// </summary>
        /// <param name="data">�ݔ��f�[�^</param>
        protected abstract void LoadSpecificData(Data data);

        private void Update()
        {
            Think();
        }

        /// <summary>
        /// Update�Ŏ��s����܂�
        /// </summary>
        protected abstract void Think();

        protected abstract void Pause();
        protected abstract void Resume();
    }
}