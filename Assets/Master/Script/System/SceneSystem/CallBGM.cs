using DCFrameWork.MainSystem;
using UnityEngine;

namespace DCFrameWork
{
    public class CallBGM : MonoBehaviour
    {
        [SerializeField] int soundIndex;
        public void Initialize()=>
            GameBaseSystem.mainSystem.PlaySound(soundIndex, SoundKind.BGM);

        [ContextMenu("playBGM")]
        public void ChangeBGM()=>
            GameBaseSystem.mainSystem.PlaySound((soundIndex++)%3, SoundKind.BGM);
    }
}
