using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace DCFrameWork.MainSystem
{
    public class SaveDataManager
    {
        private const byte _key = 173;
        private static GameSaveData _saveData = new();
        public static Action<GameSaveData> OnSaveDataChanged;

        public static GameSaveData SaveData
        {
            get => _saveData;
            private set => _saveData = value;
        }
        private static SettingSaveData _settingSaveData = null;
        public static Action<SettingSaveData> OnSettingSaveDataChanged;
        public static SettingSaveData SettingSaveData
        {
            get => _settingSaveData;
            private set => _settingSaveData = value;
        }

        static int _storyKey = 0;
        public static int StoryKey { get => _storyKey; private set => _storyKey = value; }

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
            (GameSaveData data_g, SettingSaveData data_s) = (JsonUtility.FromJson<GameSaveData>(Encryption(PlayerPrefs.GetString("GemeSaveData"))),
                JsonUtility.FromJson<SettingSaveData>(Encryption(PlayerPrefs.GetString("SettintgSaveData"))));

            if (data_g == null)
                data_g = new GameSaveData();
            if (data_s == null)
                data_s = new SettingSaveData();

            SaveData = data_g;
            SettingSaveData = data_s;
            _storyKey = data_g.StoryKey;
            return (data_g, data_s);
        }

        static string Encryption(string data) =>
            new string(data.Select(x => (char)(x ^ _key)).ToArray());

        public static void AddStoryKey(int value) => StoryKey += value;
    }

    [Serializable]
    public class GameSaveData
    {
        public string SaveDate = "";
        public int PowerUpItemValue = 0;
        public List<int> PowerUpDatas = Enumerable.Repeat(0, Enum.GetValues(typeof(DefenseObjectsKind)).Length).ToList();
        public int EventFlag = 0;
        public int StoryKey = 0;
    }
    [Serializable]
    public class SettingSaveData
    {
        public int MasterVolume = 50;
        public int SoundEffectVolume = 50;
        public int BGMVolume = 50;
        public int VoiceVolume = 50;
    }
}