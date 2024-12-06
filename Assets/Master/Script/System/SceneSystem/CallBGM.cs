using DCFrameWork.MainSystem;
using UnityEngine;

namespace DCFrameWork
{
    public class CallBGM : MonoBehaviour
    {
        public void Initialize(int soundIndex)=>
            GameBaseSystem.mainSystem.PlaySound(soundIndex, SoundKind.BGM);
    }
}
