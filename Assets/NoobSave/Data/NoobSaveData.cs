using System.Text;
using EasyButtons;
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
                if (_instance.savePath == "")
                    _instance.savePath = SAVE_PATH;
#if UNITY_EDITOR
                AssetDatabase.CreateAsset(_instance, _instance.savePath);
                AssetDatabase.SaveAssets();
#endif
                
                return _instance;
            }
        }

        private void OnEnable()
        {
           ResetPaths();
        }

        [Button(nameof(ResetPaths))]
        private void ResetPaths()
        {
            if (saveFileName == "")
            {
                saveFileName = SAVE_FILENAME;
                Debug.Log("Resetting save file name to default: " + saveFileName);
            }

            if (saveFilePath == "")
            {
                saveFilePath = Application.persistentDataPath + $"/{saveFileName}.json";
                Debug.Log("Resetting save file path to default: " + saveFilePath);
            }
        }

        [Button(nameof(TestArea))]
        public void TestArea(string key)
        {
            NoobSaveMain.Load(true);
            Debug.Log(key+" <color=red>Key:</color>"+NoobSaveMain.GetSave<string>(key));
        }
        
    }
}