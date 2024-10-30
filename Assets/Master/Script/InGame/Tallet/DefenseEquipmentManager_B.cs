using DCFrameWork.MainSystem;
using UnityEngine;

namespace DCFrameWork.DefenseEquipment
{
    public abstract class DefenseEquipmentManager_B<Data> : MonoBehaviour, IPausable where Data : DefenseEquipmentData_B
    {
        [SerializeField]
        private DefenseEquipmentDataBase _data;

        [Range(1, 5f)]
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
            LoadCommonData(Level);
            GameBaseSystem.mainSystem.AddPausableObject(this as IPausable);
        }

        private void OnDestroy()
        {
            GameBaseSystem.mainSystem.RemovePausableObject(this as IPausable);
        }

        private void LoadCommonData(int level)
        {
            Data data = _data.DataLevelList[level - 1] as Data;

            _attack = data.Attack;
            _rate = data.Rate;
            _range = data.Range;
            _critical = data.Critical;

            LoadSpecificData(data);
        }

        /// <summary>
        /// �ݒ肵���^�p�����[�^�ɑΉ�������p�ϐ��������Ă�������
        /// </summary>
        /// <param name="data">���x���ɉ������ݔ��f�[�^</param>
        protected virtual void LoadSpecificData(Data data) { }

        private void Update()
        {
            Think();
        }

        /// <summary>
        /// Update�Ŏ��s����܂�
        /// </summary>
        protected abstract void Think();

        #region �|�[�Y����
        void IPausable.Pause() => Pause();
        void IPausable.Resume() => Resume();
        protected abstract void Pause();
        protected abstract void Resume();
        #endregion
    }
}