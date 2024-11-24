using DCFrameWork.MainSystem;
using UnityEngine;

namespace DCFrameWork.DefenseEquipment
{
    public abstract class DefenseEquipmentManager_B<Data> : MonoBehaviour, IPausable where Data : DefenseEquipmentData_B
    {
        [SerializeField]
        private DefenseEquipmentDataBase _dataBase;

        private Data _defenseEquipmentData;
        protected Data DefenseEquipmentData { get => _defenseEquipmentData; }

        [Range(1, 5f)]
        protected int _level = 1;

        private void Start()
        {
            if (_dataBase is null)
                Debug.Log("データがありません");
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
            if ((_dataBase is null).CheckLog($"{gameObject.name}にデータがありません")) return;

            if (_dataBase.DataLevelList.Count < level) return;
            Data data = _dataBase.DataLevelList[level - 1] as Data;
            if ((data is null).CheckLog($"{gameObject.name}のデータがキャストできません")) return;
            _defenseEquipmentData = data;

            LoadSpecificData(data);
        }

        /// <summary>
        /// 設定した型パラメータに対応した処理を行ってください
        /// </summary>
        /// <param name="data">レベルに応じた設備データ</param>
        protected virtual void LoadSpecificData(Data data) { }

        private void Update()
        {
            Think();
        }

        /// <summary>
        /// Updateで実行されます
        /// </summary>
        protected abstract void Think();

        #region ポーズ処理
        void IPausable.Pause() => Pause();
        void IPausable.Resume() => Resume();
        protected abstract void Pause();
        protected abstract void Resume();
        #endregion
    }
}