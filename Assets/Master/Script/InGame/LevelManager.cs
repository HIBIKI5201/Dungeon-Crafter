using System;
using UnityEngine;

namespace DCFrameWork
{
    public class LevelManager : MonoBehaviour
    {
        public Action OnLevelChanged;
        public Action OnExperianceGained;
        private float _experiancePoint = 0;
        private float _nextLevelRequireExperiancePoint = 100;

        public void Initialize()
        {
            
        }
    }
}
