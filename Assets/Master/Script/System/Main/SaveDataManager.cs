using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace DCFrameWork.MainSystem
{
    public class SaveDataManager
    {
        private const byte _key = 173;
        private static GameSaveData _saveData = null;
        public static Action<GameSaveData> OnSaveDataChanged;
        public static GameSaveData SaveData
        {
            get => _saveData;
            set
            {
                _saveData = value;
                OnSaveDataChanged?.Invoke(value);
            }
        }
        private static SettingSaveData _settingSaveData = null;
        public static Action<SettingSaveData> OnSettingSaveDataChanged;
        public static SettingSaveData SettingSaveData
        {
            get => _settingSaveData;
            set
            {
                _settingSaveData = value;
                OnSettingSaveDataChanged?.Invoke(value);
            }
        }

        public static void Save()
        {
            SaveData.SaveDate = DateTime.Now.ToString("O");
            PlayerPrefs.SetString("GameSaveData", Encryption(JsonUtility.ToJson(SaveData)));
        }

        public static void SettingSave()
        {
            PlayerPrefs.SetString("SettingSaveData", Encryption(JsonUtility.ToJson(SettingSaveData)));
        }

        public static (GameSaveData, SettingSaveData) Load()
        {
            return (JsonUtility.FromJson<GameSaveData>(Encryption(PlayerPrefs.GetString("GemeSaveData"))),
                JsonUtility.FromJson<SettingSaveData>(Encryption(PlayerPrefs.GetString("SettintgSaveData"))));
        }
        static string Encryption(string data) =>
            new string(data.Select(x => (char)(x ^ _key)).ToArray());
    }
    [System.Serializable]
    public class GameSaveData
    {
        public string SaveDate = "";
        public int PowerUpItemValue = 0;
        public List<int> PowerUpDatas = new List<int>();
        public int EventFlag = 0;
    }
    [System.Serializable]
    public class SettingSaveData
    {
        public int MasterVolume = 50;
        public int SoundEffectVolume = 50;
        public int BGMVolume = 50;
    }
}