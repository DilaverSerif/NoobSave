using System.IO;
using UnityEditor;
using UnityEngine;

namespace NoobSave
{
    public static class NoobSaveEditor
    {
        private static NoobSaveData _instance = NoobSaveData.Instance;
        private static string SAVE_PATH => NoobSaveData.Instance.savePath;

        [MenuItem("Tools/Noob Save/Runtime Load")]
        public static void RunTimeLoad()
        {
            NoobSaveMain.Load();
        }

        [MenuItem("Tools/Noob Save/Runtime Save")]
        public static void RunTimeSave()
        {
            NoobSaveMain.Save();
        }

        [MenuItem("Tools/Noob Save/Create NoobSaveData")]
        public static void CreateAsset()
        {
            if (!Directory.Exists(SAVE_PATH))
            {
                Directory.CreateDirectory(SAVE_PATH);
                Debug.Log("Created NoobSaveData folder");
            }
            
            _instance.name = nameof(NoobSaveData);
            AssetDatabase.CreateAsset(_instance, SAVE_PATH);
            AssetDatabase.SaveAssets();

            Debug.Log("Created NoobSaveData asset");
            Selection.activeObject = _instance;
        }

        [MenuItem("Tools/Noob Save/Select Save File")]
        public static void SelectSaveFile()
        {
            Selection.activeObject = _instance;
        }

        [MenuItem("Tools/Noob Save/Delete Save")]
        public static void DeleteSave()
        {
            var filePath = Application.persistentDataPath + $"/{_instance.saveFileName}.json";
            if (File.Exists(filePath))
                File.Delete(filePath);

            if (_instance.deletePlayerPrefsOnLoad)
                PlayerPrefs.DeleteAll();

            _instance.saveData.ClearData();
            AssetDatabase.SaveAssets();
        }

        [MenuItem("Tools/Noob Save/Open Save File")]
        public static void OpenFolder()
        {
            var filePath = _instance.saveFilePath;
            if (File.Exists(filePath))
            {
                EditorUtility.RevealInFinder(filePath);
            }
        }
    }
}