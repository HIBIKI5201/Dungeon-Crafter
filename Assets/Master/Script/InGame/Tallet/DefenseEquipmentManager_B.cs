using DCFrameWork.MainSystem;
using UnityEngine;

namespace DCFrameWork.DefenseEquipment
{
    public abstract class DefenseEquipmentManager_B<Data> : MonoBehaviour, IPausable where Data : DefenseEquipmentData_B
    {
        [SerializeField]
        private DefenseEquipmentDataBase _dataBase;

        [Range(1, 5f)]
        protected int _level = 1;

        #region ���ʃX�e�[�^�X
        protected float _attack;
        protected float _rate;
        protected float _critical;
        #endregion

        private void Start()
        {
            if (_dataBase is null)
                Debug.Log("�f�[�^������܂���");
            _level = 1;
            LoadCommonData(_level);
            GameBaseSystem.mainSystem?.AddPausableObject(this);
            Start_SB();
        }

        protected virtual void Start_SB() { }

        private void OnDestroy()
        {
            GameBaseSystem.mainSystem?.RemovePausableObject(this);
        }

        private void LoadCommonData(int level)
        {
            if ((_dataBase is null).CheckLog($"{gameObject.name}�Ƀf�[�^������܂���")) return;

            if (_dataBase.DataLevelList.Count < level) return;
            Data data = _dataBase.DataLevelList[level - 1] as Data;
            if ((data is null).CheckLog($"{gameObject.name}�̃f�[�^���L���X�g�ł��܂���")) return;

            _attack = data.Attack;
            _rate = data.Rate;
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