using System.Collections.Generic;
using UnityEngine;
namespace DCFrameWork.MainSystem
{
    public class SaveDataManager : MonoBehaviour
    {
        private static GameSaveData _saveData = null;
        static string _key = "TestKey";
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
            var saveDataStr=JsonUtility.ToJson(SaveData);
            var xorData = "";
            for (int i = 0; i < saveDataStr.Length; i++)
            {
                xorData += (byte)saveDataStr[i]^(byte)_key[i%_key.Length-1];
            }
            PlayerPrefs.SetString("GameSaveData",saveDataStr);
        }

        public static void SettingSave()
        {
            var settingSaveDataStr=JsonUtility.ToJson(SettingSaveData);
            Debug.Log(settingSaveDataStr);

            PlayerPrefs.SetString("SettingSaveData",settingSaveDataStr);
        }

        public static (GameSaveData, SettingSaveData) Load()
        {
            var gameSaveDataStr = PlayerPrefs.GetString("GemeSaveData");
            var settingSaveDataStr = PlayerPrefs.GetString("SettintgSaveData");
            GameSaveData gameSaveData = JsonUtility.FromJson<GameSaveData>(gameSaveDataStr);
            SettingSaveData settingSaveData = JsonUtility.FromJson<SettingSaveData>(settingSaveDataStr);
            return (gameSaveData, settingSaveData); //‰¼‚Ì–ß‚è’l
        }
    }
    [System.Serializable]
    public class GameSaveData
    {
        public string SaveDate;
        public int PowerUpItemValue;
        public List<int> PowerUpDatas;
        public int EventFlag;
    }
    [System.Serializable]
    public class SettingSaveData
    {
        public int MasterVolume;
        public int SoundEffectVolume;
        public int BGMVolume;
    }
}