using DCFrameWork.UI;
using System.Threading.Tasks;
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