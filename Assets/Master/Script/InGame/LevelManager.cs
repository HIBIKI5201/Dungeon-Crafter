using System;
using UnityEngine;

namespace DCFrameWork
{
    public class LevelManager : MonoBehaviour
    {

        private int _level = 1;
        public int Level { get => _level; }

        public Action<int> OnLevelChanged;
        public Action<float> OnExperianceGained;

        private float _experiancePoint = 0;
        private float _nextLevelRequireExperiancePoint = 100;


        [SerializeField] DropTableData _dropTable;
        [SerializeField] int _levelUpGachaCount = 3;

        private PlayerManager _playerManager;

        public void Initialize()
        {
            _playerManager = GetComponent<PlayerManager>();
            _level = 1;
            _experiancePoint = 0;
            OnLevelChanged += (a) => GetRandomDefenseObj();
        }

        public void AddExperiancePoint(float point)
        {
            _experiancePoint += point;
            if (_nextLevelRequireExperiancePoint < _experiancePoint)
            {
                _experiancePoint -= _nextLevelRequireExperiancePoint;
                _level = _level + 1;
                OnLevelChanged?.Invoke(_level);
            }
            OnExperianceGained?.Invoke(_experiancePoint);
        }
        public void ChangeDropTable(DropTableData dropTable) => _dropTable = dropTable;

        private void GetRandomDefenseObj()
        {
            var collection = _dropTable.GetRandomDefenseObj(_levelUpGachaCount);
            foreach (var item in collection)
            {
                _playerManager.SetDefenseObject(item);
            }
        }
    }
}
