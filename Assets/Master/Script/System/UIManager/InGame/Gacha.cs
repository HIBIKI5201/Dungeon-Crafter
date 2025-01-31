using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace DCFrameWork
{
    [UxmlElement]
    public partial class Gacha : VisualElement_B
    {
        VisualTreeAsset _card;
        VisualElement _gachaWindow;
        public event Action<TemplateContainer> OnGacha;
        public VisualTreeAsset Card { set => _card = value; }
        public event Action<DefenseObjectsKind, int> OnGachaClose;
        public event Action<bool> OnMouseCursor;
        public Gacha() : base("UXML/InGame/Gacha") { }

        //ガチャの内容の生成
        public void GachaBake(List<InventoryData> list)
        {
            _gachaWindow.RemoveFromClassList("gacha-close");
            _gachaWindow.Clear();
            foreach (var turret in list)
            {
                var uiInstance = _card.Instantiate();
                uiInstance.style.width = Length.Percent(23);
                uiInstance.style.height = Length.Percent(26);
                _gachaWindow.Add(uiInstance);
                uiInstance.Q<Label>("EquipmentLebel").text = turret.Level.ToString();
                uiInstance.Q<Label>("DefenceEquipment").text = turret.Name;
                uiInstance.Q<Label>("DefenceEquipmentText").text = turret.Explanation;
                uiInstance.Q<VisualElement>("EquipmentCard").RegisterCallback<ClickEvent>(x =>
                {
                    OnGachaClose?.Invoke(turret.Kind, turret.Level);
                    _gachaWindow.AddToClassList("gacha-close");
                    Debug.Log("GachaClose");
                });
            }
        }

        protected override Task Initialize_S(TemplateContainer container)
        {
            //UXMLの取得
            _gachaWindow = container.Q<VisualElement>("Gacha");
            //UIにマウスが乗った時離れたときのイベント
            _gachaWindow.RegisterCallback<MouseEnterEvent>(x => OnMouseCursor?.Invoke(true));
            _gachaWindow.RegisterCallback<MouseLeaveEvent>(x => OnMouseCursor?.Invoke(false));
            //ガチャのスタイルを初期でクローズにする
            _gachaWindow.AddToClassList("gacha-close");

            return Task.CompletedTask;
        }
    }
}
