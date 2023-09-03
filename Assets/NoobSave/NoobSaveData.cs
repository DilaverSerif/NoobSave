using System.Text;
using UnityEditor;
using UnityEngine;

namespace NoobSave
{
    public class NoobSaveData : ScriptableObject
    {
        private const string SAVE_FILENAME = "save";
        private const string SAVE_PATH = "Assets/NoobSave/Resources/NoobSaveData.asset";

        [Header("Paths")] public string saveFilePath;
        public string savePath = "Assets/NoobSave/Resources/NoobSaveData.asset";
        [Header("Settings")] public string saveFileName;
        public bool deletePlayerPrefsOnLoad = true;
        public bool usingEncrypt = false;
        public string encryptionKey = "an0vwOQC0xuUZtgZZgUwCU61QxOtDb6i";
        private static NoobSaveData _instance;
        [Header("Save File")] public SaveData saveData;
        public static NoobSaveData Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;
                _instance = Resources.Load<NoobSaveData>(nameof(NoobSaveData));

                if (_instance != null)
                    return _instance;

                _instance = CreateInstance<NoobSaveData>();
                _instance.name = "NoobSaveData";
                AssetDatabase.CreateAsset(_instance, _instance.savePath);
                AssetDatabase.SaveAssets();

                return _instance;
            }
        }

        private void OnEnable()
        {
            if (saveFileName == "")
                saveFileName = SAVE_FILENAME;

            if (saveFilePath == "")
                saveFilePath = Application.persistentDataPath + $"/{saveFileName}.json";
        }
        
    }
}