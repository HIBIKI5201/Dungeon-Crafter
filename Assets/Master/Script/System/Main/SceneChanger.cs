using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace DCFrameWork.MainSystem
{
    public class SceneChanger : MonoBehaviour
    {
        static Dictionary<SceneKind, string> _sceneNameDict = new()
        {
            {SceneKind.Home, "" },
            {SceneKind.Ingame_1, ""}
        };

        private static string _currentSceneName;
        private static bool _isLoading;
        public static IEnumerator LoadScene(SceneKind kind)
        {
            if (_isLoading) yield break;
            _isLoading = true;
            yield return SceneManager.LoadSceneAsync(_sceneNameDict[kind], LoadSceneMode.Additive);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(_sceneNameDict[kind]));
            yield return SceneManager.UnloadSceneAsync(_currentSceneName);
            _currentSceneName = _sceneNameDict[kind];
            _isLoading = false;
        }
    }

    public enum SceneKind
    {
        Home,
        Ingame_1,
    }
}