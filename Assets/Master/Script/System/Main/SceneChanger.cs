using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
namespace DCFrameWork.MainSystem
{
    public class SceneChanger
    {
        private static Dictionary<SceneKind, string> _sceneNameDict = new()
        {
            {SceneKind.Home, "Master_Home" },
            { SceneKind.Story, "Master_Story"},
            {SceneKind.Ingame_0, "Master_Ingame_Stage0"},
            {SceneKind.Ingame_1, "Master_Ingame_Stage1"},
        };
        public static string SetCurrentSceneName() => _currentSceneName = SceneManager.GetActiveScene().name;
        private static string _currentSceneName = SceneManager.GetActiveScene().name;
        private static bool _isLoading;
        public static IEnumerator LoadScene(SceneKind kind)
        {
            if (_isLoading) yield break;
            _isLoading = true;

            try
            {
                yield return SceneManager.UnloadSceneAsync(_currentSceneName);

                _currentSceneName = _sceneNameDict[kind];
                yield return SceneManager.LoadSceneAsync(_currentSceneName, LoadSceneMode.Additive);
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(_currentSceneName));
            }
            finally
            {
                _isLoading = false;
            }               
          


        }
    }

    public enum SceneKind
    {
        Home,
        Story,
        Ingame_0,
        Ingame_1,
    }
}