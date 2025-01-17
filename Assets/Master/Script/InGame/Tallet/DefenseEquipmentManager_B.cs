using DCFrameWork.MainSystem;
using System;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;


namespace DCFrameWork.DefenseEquipment
{
    public abstract class DefenseEquipmentManager_B<Data> : MonoBehaviour, ITurret where Data : DefenseEquipmentData_B
    {
        [SerializeField]
        private DefenseEquipmentDataBase _dataBase;
        DefenseEquipmentDataBase ITurret.Data { get => _dataBase; set => _dataBase = value; }
        [SerializeField]
        private GameObject _cylinder;

        private Data _defenseEquipmentData;
        protected Data DefenseEquipmentData { get => _defenseEquipmentData; }

        protected float Attack { get => DefenseEquipmentData.Attack + _reinforceStatus.Attack; }
        protected float Range { get => DefenseEquipmentData.Range + _reinforceStatus.Range; }
        protected float Rate { get => DefenseEquipmentData.Rate + _reinforceStatus.Rate; }
        protected float Critical { get => DefenseEquipmentData.Critical + _reinforceStatus.Critical; }

        private ReinforceStatus _reinforceStatus = ReinforceStatus.Default;

        [Range(1, 5f)]
        protected int _level = 1;

        PlayerManager _playerManager;


        private void Start()
        {
            if (_dataBase is null)
                Debug.Log("データがありません");
            _level = 1;
            LoadCommonData(_level);
            GameBaseSystem.mainSystem?.AddPausableObject(this);
            Start_SB();
            _playerManager = FindAnyObjectByType<PlayerManager>();
        }

        protected virtual void Start_SB() { }

        private void OnDestroy()
        {
            GameBaseSystem.mainSystem?.RemovePausableObject(this);
        }
        public void Reinforce(ReinforceStatus status)
        {
            _reinforceStatus = new(status.Attack, status.Rate, status.Range, status.Critical);
            RangeSet(Range);
        }

        private void LoadCommonData(int level)
        {
            if ((_dataBase is null).CheckLog($"{gameObject.name}にデータがありません")) return;

            if (_dataBase.DataLevelList.Count < level) return;
            Data data = _dataBase.DataLevelList[level - 1] as Data;
            if ((data is null).CheckLog($"{gameObject.name}のデータがキャストできません")) return;
            _defenseEquipmentData = data;
            RangeSet(Range);
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

        protected private void RangeSet(float range)
        {
            var coll = GetComponent<SphereCollider>();
            coll.radius = range;
            if (_cylinder)
                _cylinder.transform.localScale = new Vector3(coll.radius * 2, _cylinder.transform.localScale.y, coll.radius * 2);
        }
        /// <summary>
        /// タレットのレベルアップ機能
        /// </summary>
        public void LevelUp()
        {
            if (_level == 5)
            {
                Debug.Log("強化上限に達しました。");
                return;
            }
            if (_playerManager.ChangeGold(-DefenseEquipmentData.UseGold))
            {
                _level = Mathf.Min(5, _level + 1);
                LoadCommonData(_level);
            }
            else if (!_playerManager.ChangeGold(-DefenseEquipmentData.UseGold))
            {
                Debug.Log("お金が足りません。");
            }
        }

        /// <summary>
        /// 次のレベルのステータスが格納されたデータを返すメソッド
        /// </summary>
        /// <returns></returns>
        public DefenseEquipmentData_B NextStatus()
        {
            if (_level != 5)
            {
                return _dataBase.DataLevelList[_level];
            }
            return null;
        }


        #region ポーズ処理
        void IPausable.Pause() => Pause();
        void IPausable.Resume() => Resume();
        protected abstract void Pause();
        protected abstract void Resume();

        void ITurret.Reinforce(ReinforceStatus status) => Reinforce(status);
        #endregion
    }
    public interface ITurret : IPausable
    {
        void Reinforce(ReinforceStatus status);
        DefenseEquipmentDataBase Data { get; protected set; }

        GameObject gameObject { get; }
        Transform transform { get; }
    }
}
