using Data;
using UnityEngine;

namespace Controllers
{
    public class SaveController
    {
        public PlayerData PlayerData { get; private set; }

        private const string SaveKey = "Save";
        
        public void Load()
        {
            var prefs = PlayerPrefs.GetString(SaveKey, string.Empty);

            if (string.IsNullOrEmpty(prefs))
            {
                PlayerData = new PlayerData();
            }
            else
            {
                PlayerData = JsonUtility.FromJson<PlayerData>(prefs);
            }
        }

        public void Save()
        {
            var json = JsonUtility.ToJson(PlayerData);
            
            PlayerPrefs.SetString(SaveKey, json);
        }

        public void Clear()
        {
            PlayerData = new PlayerData();

            PlayerPrefs.DeleteKey(SaveKey);
        }
    }
}