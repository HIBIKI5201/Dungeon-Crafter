using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeUIManager : MonoBehaviour
{
    HomeUIState _homeUIState = HomeUIState.Title;
    private void Awake()
    {
    }
    private void LateUpdate()
    {

    }
    public HomeUIState State
    {
        get { return _homeUIState;}
        set
        {
            if(_homeUIState != value)
            {
                _homeUIState=value;
                OnStateChenge();
            }
        }
    }
    void OnStateChenge()
    {
        switch (_homeUIState)
        {
            case HomeUIState.Title:
                OnTitle();
                break;
            case HomeUIState.Scenario:
                OnScenario();
                break;
            case HomeUIState.MainMenu:
                OnMainMenu();
                break;
        }
    }
    void OnTitle()
    {

    }
    void OnScenario()
    {

    }
    void OnMainMenu()
    {

    }
}
public enum HomeUIState
{
    Title,
    Scenario,
    MainMenu,
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
