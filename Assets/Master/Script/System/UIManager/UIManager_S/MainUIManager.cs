using DCFrameWork.UI;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.UIElements;

public class MainUIManager : UIManager_B
{
    BlackOut _blackOut;
    protected override async Task LoadDocumentElement(VisualElement root)
    {
        _blackOut = root.Q<BlackOut>("BlackOut");
        await _blackOut.InitializeTask;
    }
    public void BlackOut(bool blackOut)
    {
        _blackOut.IsBlackOut = blackOut;
    }
}