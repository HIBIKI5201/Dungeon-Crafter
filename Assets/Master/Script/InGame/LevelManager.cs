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



        public void Initialize()
        {
            _level = 1;
            _experiancePoint = 0;
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
    }
}
