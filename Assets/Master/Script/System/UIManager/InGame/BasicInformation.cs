using DCFrameWork;
using System;
using System.Threading.Tasks;
using UnityEngine.UIElements;

[UxmlElement]
public partial class BasicInformation : VisualElement_B
{
    //UI要素
    private GuageMesh _guageMesh;
    private VisualElement _eXPGuage;
    private Label _eXPText;
    private Label _phaseText;
    private Label _goldText;

    private Button _menuButton;
    private float _exp;
    private float _level;
    //イベント
    public event Action<bool> OnMouseCursor;
    //プロパティ
    public float Exp
    {
        set
        {
            _exp = value;
            EXPTextUpdate();
        }
    }
    public float Level
    {
        set
        {
            _level = value;
            EXPTextUpdate();
        }
    }
    public float Money
    {
        set
        {
            _goldText.text = "所持金:" + value;
        }
    }
    public GuageMesh GuageMesh { get => _guageMesh; set => _guageMesh = value; }
    public VisualElement EXPGuage { get => _eXPGuage; set => _eXPGuage = value; }
    public string EXPText { get => _eXPText.text; set => _eXPText.text = value; }
    public string PhaseText { get => _phaseText.text; set => _phaseText.text = value; }
    public Button MenuButton { get => _menuButton; set => _menuButton = value; }
    public BasicInformation() : base("UXML/InGame/BasicInformation") { } 
    public void EXPTextUpdate()
    {
        string expText = "EXP:" + _exp.ToString() + Environment.NewLine + "Level:" + _level.ToString();
        EXPText = expText;
    }
    public void EXPGuageUpdate(float persentNormal)
    {
        float persent = persentNormal / 100f * 87;
        _eXPGuage.style.width = Length.Percent(persent);
    }

    protected override Task Initialize_S(TemplateContainer container)
    {
        // UI要素の取得
        _guageMesh = container.Q<GuageMesh>("PhaseGuage");
        _eXPGuage = container.Q<VisualElement>("EXPGuage");
        _eXPText = container.Q<Label>("EXPText");
        _phaseText = container.Q<Label>("PhaseText");
        _menuButton = container.Q<Button>("MenuButton");
        _goldText = container.Q<Label>("MoneyText");

        //UIの上にマウスが乗ったとき離れたときに発火する
        _menuButton.RegisterCallback<MouseEnterEvent>(x => OnMouseCursor?.Invoke(true));
        _menuButton.RegisterCallback<MouseLeaveEvent>(x => OnMouseCursor?.Invoke(false));

        return Task.CompletedTask;
    }
}