using DCFrameWork.SceneSystem;
using System;
using UnityEngine;

namespace DCFrameWork.UI
{
    public class HomeUIManager : MonoBehaviour
    {
        HomeWindowState _homeUIState = HomeWindowState.Title;
        public HomeWindowState State
        {
            get { return _homeUIState; }
            set
            {
                if (_homeUIState != value)
                {
                    _homeUIState = value;
                    OnStateChenge();
                }
            }
        }

        void OnStateChenge()
        {
            Action action = _homeUIState switch
            {
                HomeWindowState.Title => OnTitle,
                HomeWindowState.Story => OnStory,
                HomeWindowState.MainMenu => OnMainMenu,
                _ => null
            };
            action?.Invoke();
        }
        void OnTitle()
        {

        }
        void OnStory()
        {

        }
        void OnMainMenu()
        {

        }
    }

    class TitleElement
    {

    }
    class ScenarioElement
    {

    }
    class MainManuElement
    {

    }
}