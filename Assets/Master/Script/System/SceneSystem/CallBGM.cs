using DCFrameWork.MainSystem;
using UnityEngine;

namespace DCFrameWork
{
    public class CallBGM : MonoBehaviour
    {
        [SerializeField]
        int soundIndex;
        [SerializeField]
        BGMMode bgmMode;
        public void Initialize()=>
            GameBaseSystem.mainSystem.PlayBGM(soundIndex, bgmMode);

        [ContextMenu("PlayBGM")]
        void FadeBGM()
        {
            GameBaseSystem.mainSystem.PlayBGM(soundIndex++, BGMMode.CrossFade);
        }
    }
}
