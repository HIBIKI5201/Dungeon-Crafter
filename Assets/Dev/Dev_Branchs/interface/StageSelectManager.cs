using UnityEditor.Build.Content;
using UnityEngine;
using DCFrameWork.MainSystem;

namespace DCFrameWork
{
    public class StageSelectManager : MonoBehaviour
    {
        public void StageSelect(SceneKind kind)
        {
            SceneChanger.LoadScene(kind);
        }
    }
}
