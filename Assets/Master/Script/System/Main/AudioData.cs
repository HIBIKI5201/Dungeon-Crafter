using DCFrameWork.MainSystem;
using System.Collections.Generic;
using UnityEngine;

namespace DCFrameWork
{

    [CreateAssetMenu(fileName = "AudioDataBase", menuName = "GameData/AudioDataBase")]
    public class AudioDataBase : ScriptableObject
    {
        public List<AudioData> audioDatas = new();
    }
}
