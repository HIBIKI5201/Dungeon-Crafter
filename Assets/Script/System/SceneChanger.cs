using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DCFrameWork.MainSystem
{
    public class SceneChanger : MonoBehaviour
    {
        static Dictionary<SceneKind, string> _sceneNameDict = new()
        {
            {SceneKind.Home, "" },
            {SceneKind.Ingame_1, ""}
        };

        public static IEnumerator LoadScene(SceneKind kind)
        {
            yield return null;
        }
    }

    public enum SceneKind
    {
        Home,
        Ingame_1,
    }
}