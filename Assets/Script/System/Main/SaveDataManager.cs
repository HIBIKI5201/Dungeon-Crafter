using System.Collections.Generic;
using UnityEngine;
namespace DCFrameWork.MainSystem
{
    public class SaveDataManager : MonoBehaviour
    {
        private static GameSaveData _saveData = null;
        public static GameSaveData SaveData
        {
            get => _saveData;
            set=> _saveData = value;
        }
        private static SettingSaveData _settingSaveData = null;
        public static SettingSaveData SettingSaveData
        {
            get => _settingSaveData;
            set => _settingSaveData = value;
        }

        public static void Save()
        {

        }

        public static void SettingSave()
        {

        }

        public static (GameSaveData, SettingSaveData) Load()
        {
            return (null, null); //‰¼‚Ì–ß‚è’l
        }
    }

    public class GameSaveData
    {
        public string SaveDate;
        public int PowerUpItemValue;
        public List<int> PowerUpDatas;
        public int EventFlag;
    }

    public class SettingSaveData
    {
        public int MasterVolume;
        public int SoundEffectVolume;
        public int BGMVolume;
    }
}