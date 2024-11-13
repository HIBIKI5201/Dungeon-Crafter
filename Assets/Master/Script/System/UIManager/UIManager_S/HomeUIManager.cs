using DCFrameWork.SceneSystem;
using System;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.UIElements;
using static UnityEditor.Recorder.OutputPath;
using UnityEditorInternal;

namespace DCFrameWork.UI
{
    public class HomeUIManager : MonoBehaviour
    {
        VisualElement _root;
        HomeWindowState _homeUIState = HomeWindowState.Title;
        TemplateContainer _title;
        TemplateContainer _menu;
        TemplateContainer _stage;
        TemplateContainer _defenceEquipment;
        VisualElement _titleBackGround;
        Button _stageSelectButton;
        Button _defenceEquipmentButton;
        Button _titleButton;
        Button _stageone;
        Button _stageReturnButton;
        Button _equipmentReturnButton;

        private void Start()
        {
            var uiDocu = GetComponent<UIDocument>();
            _root = uiDocu.rootVisualElement;
            _title = _root.Q<TemplateContainer>("title-window");
            _titleBackGround = _title.Q<VisualElement>("TitleWindow");
            _titleBackGround.RegisterCallback<ClickEvent>(x => { State = HomeWindowState.MainMenu; });
            _menu = _root.Q<TemplateContainer>("menu-window");
            _stageSelectButton = _menu.Q<Button>("stage-select-button");
            _defenceEquipmentButton = _menu.Q<Button>("defence-equipment-button");
            _titleButton = _menu.Q<Button>("title-button");
            _stageSelectButton.RegisterCallback<ClickEvent>(x => {State = HomeWindowState.StageSelect; });
            _defenceEquipmentButton.RegisterCallback<ClickEvent>(x => {State = HomeWindowState.DefenceEquipment; });
            _titleButton.RegisterCallback<ClickEvent>(x => {State= HomeWindowState.Title; });
            _stage = _root.Q<TemplateContainer>("stage-select");
            _stageone = _stage.Q<Button>("stage-one-button");
            _stageReturnButton = _stage.Q<Button>("return-button");
            _stageReturnButton.RegisterCallback<ClickEvent>(x => { State = HomeWindowState.MainMenu; });
            _stageone.RegisterCallback<ClickEvent>(x => { });
            _defenceEquipment = _root.Q<TemplateContainer>("defence-equipment");
            _equipmentReturnButton = _defenceEquipment.Q<Button>("return-button");
            _equipmentReturnButton.RegisterCallback<ClickEvent>(x => { State = HomeWindowState.MainMenu; });
        }
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
                HomeWindowState.MainMenu => OnMainMenu,
                HomeWindowState.StageSelect => OnStageSelect,
                HomeWindowState.DefenceEquipment => OnDefenceEquipment,
                _ => null
            };
            action?.Invoke();
        }
        void OnTitle()
        {
            _root.Clear();
            _root.Insert(0, _title);
        }
        void OnMainMenu()
        {
            _root.Clear();
            _root.Insert(0, _menu);
        }
        void OnStageSelect()
        {
            _root.Clear();
            _root.Insert(0, _stage);
        }
        void OnDefenceEquipment()
        {
            _root.Clear();
            _root.Insert(0, _defenceEquipment);
        }
    }
}
