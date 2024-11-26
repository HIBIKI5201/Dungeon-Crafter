using System;
using System.Collections.Generic;
using UnityEngine;

namespace DCFrameWork.SceneSystem
{
    [CreateAssetMenu(menuName = "GameData/StoryTextData", fileName = "StoryTextData")]
    public class StoryData : ScriptableObject
    {
        [SerializeField]
        private List<StoryText> _list = new();
        public List<StoryText> StoryText { get { return _list; } }
    }

    [Serializable]
    public class StoryText
    {
        public string _character;
        public string _text;
        public string _animation;
    }
}
