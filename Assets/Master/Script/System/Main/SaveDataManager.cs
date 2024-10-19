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
            string encryptionData = new string(gameSaveDataStr.Select(x => (char)(x ^ _key)).ToArray());//ˆÃ†‰»
            PlayerPrefs.SetString("GameSaveData", encryptionData);
        }

        public static void SettingSave()
        {
            var settingSaveDataStr = JsonUtility.ToJson(SettingSaveData);
            Debug.Log(settingSaveDataStr);
            string encryptionData = new string(settingSaveDataStr.Select(x => (char)(x ^ _key)).ToArray());
            PlayerPrefs.SetString("SettingSaveData", encryptionData);
        }

        public static (GameSaveData, SettingSaveData) Load()
        {
            var gameSaveDataStr = PlayerPrefs.GetString("GemeSaveData");
            var settingSaveDataStr = PlayerPrefs.GetString("SettintgSaveData");
            string decryptGameData = new string(gameSaveDataStr.Select(x => (char)(x ^ _key)).ToArray());//•œ†
            string decryptSettingData = new string(settingSaveDataStr.Select(x => (char)(x ^ _key)).ToArray());
            GameSaveData gameSaveData = JsonUtility.FromJson<GameSaveData>(decryptGameData);
            SettingSaveData settingSaveData = JsonUtility.FromJson<SettingSaveData>(decryptSettingData);
            return (gameSaveData, settingSaveData);
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