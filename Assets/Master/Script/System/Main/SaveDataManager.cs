using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace DCFrameWork.MainSystem
{
    public class SaveDataManager : MonoBehaviour
    {
        private static GameSaveData _saveData = null;
        const byte _key = 173;
        public static GameSaveData SaveData
        {
            get => _saveData;
            set => _saveData = value;
        }
        private static SettingSaveData _settingSaveData = null;
        public static SettingSaveData SettingSaveData
        {
            get => _settingSaveData;
            set => _settingSaveData = value;
        }

        public static void Save()
        {
            var gameSaveDataStr = JsonUtility.ToJson(SaveData);
            PlayerPrefs.SetString("GameSaveData", Encryption(gameSaveDataStr));
        }

        public static void SettingSave()
        {
            var settingSaveDataStr = JsonUtility.ToJson(SettingSaveData);
            PlayerPrefs.SetString("SettingSaveData", Encryption(settingSaveDataStr));
        }

        public static (GameSaveData, SettingSaveData) Load()
        {
            var gameSaveDataStr = PlayerPrefs.GetString("GemeSaveData");
            var settingSaveDataStr = PlayerPrefs.GetString("SettintgSaveData");
            GameSaveData gameSaveData = JsonUtility.FromJson<GameSaveData>(Encryption(gameSaveDataStr));
            SettingSaveData settingSaveData = JsonUtility.FromJson<SettingSaveData>(Encryption(settingSaveDataStr));
            return (gameSaveData, settingSaveData);
        }
        static string Encryption(string data)
        {
            return new string(data.Select(x => (char)(x ^ _key)).ToArray());
        }
    }
    [System.Serializable]
    public class GameSaveData
    {
        public string SaveDate = "";
        public int PowerUpItemValue = 0;
        public List<int> PowerUpDatas=new List<int>();
        public int EventFlag=0;
    }
    [System.Serializable]
    public class SettingSaveData
    {
        public int MasterVolume=50;
        public int SoundEffectVolume=50;
        public int BGMVolume = 50;
    }
}