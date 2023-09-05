using System.IO;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

namespace NoobSave.Editor
{
    public static class NoobSaveEditor
    {
        private static NoobSaveData _instance = NoobSaveData.Instance;
        private static string SAVEPATH => NoobSaveData.Instance.savePath;

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
            if (!Directory.Exists(SAVEPATH))
            {
                Directory.CreateDirectory(SAVEPATH);
                Debug.Log("Created NoobSaveData folder");
            }
            
            _instance.name = nameof(NoobSaveData);
            AssetDatabase.CreateAsset(_instance, SAVEPATH);
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
#endif
