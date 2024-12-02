using System.Collections;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Recorder.OutputPath;

namespace DCFrameWork.UI
{
    public class InGameUIManager : UIManager_B
    {
        [SerializeField] EquipmentCardData[] testcard;
        EquipmentCardInventory _equipmentList;
        BasicInformation _basicInformation;
        EquipmentSettingUI _equipmentSettingUI;
        protected override async Task LoadDocumentElement(VisualElement root)
        {
            _equipmentList = root.Q<EquipmentCardInventory>("EquipmentCardInventory");
            _basicInformation = root.Q<BasicInformation>("BasicInformation");
            _equipmentSettingUI = root.Q<EquipmentSettingUI>("EquipmentSettingUI");
            await _equipmentList.InitializeTask;
            await _basicInformation.InitializeTask;
            await _equipmentSettingUI.InitializeTask;
            _equipmentList.ListView.itemsSource = testcard;
            Debug.Log("実行しています");
        }

        //ウェーブゲージを更新するためのメソッド
        public void WaveGuageUpdate(int waveCount, float newParsent)
        {
            _basicInformation.WaveGuege = newParsent;
            _basicInformation.WaveText = waveCount.ToString();
        }

        //防衛設備のウィンドウの表示非表示のメソッド
        public void EquipmentSettingWindowVisible(bool visible)
        {
            _equipmentSettingUI.EquipmentSettingWindowVisible = visible;
        }
    }
}