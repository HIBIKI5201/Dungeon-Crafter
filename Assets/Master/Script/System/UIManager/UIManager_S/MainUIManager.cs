using DCFrameWork.UI;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.UIElements;

public class MainUIManager : UIManager_B
{
    VisualElement _backGround;
    protected override async Task LoadDocumentElement(VisualElement root)
    {
        _backGround = root.Q<VisualElement>("BlackOut");
    }
    public void BlackOut(bool blackOut)
    {
        if (blackOut)
        {
            _backGround.RemoveFromClassList("black-out-false");
            _backGround.AddToClassList("black-out-true");
            return;
        }
        _backGround.RemoveFromClassList("black-out-true");
        _backGround.AddToClassList("black-out-false");
    }
}