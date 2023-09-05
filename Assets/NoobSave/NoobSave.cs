using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FullSerializer;
using NoobSave.Interfaces;
using UnityEngine;
using Object = UnityEngine.Object;

namespace NoobSave
{
    public static class NoobSaveMain
    {
        public static SaveData SaveData
        {
            get => NoobSaveData.Instance.saveData;
            set => NoobSaveData.Instance.saveData = value;
        }

        private static readonly fsSerializer Serializer = new();

        public static Task Save()
        {
            try
            {
                var saveObjects = Object.FindObjectsOfType<MonoBehaviour>().OfType<INoobSaveable>();

                foreach (var saveObject in saveObjects)
                    saveObject.Save();

                Serializer.TrySerialize(typeof(SaveData), NoobSaveData.Instance.saveData, out var data)
                    .AssertSuccessWithoutWarnings();

                if (NoobSaveData.Instance.usingEncrypt)
                {
                    var encryptString = NoobSaveCrypter.EncryptJson(fsJsonPrinter.CompressedJson(data),
                        NoobSaveData.Instance.encryptionKey);
                    File.WriteAllText(Application.persistentDataPath + $"/{NoobSaveData.Instance.saveFileName}.json",
                        encryptString);
                    return Task.CompletedTask;
                }

                File.WriteAllText(Application.persistentDataPath + $"/{NoobSaveData.Instance.saveFileName}.json",
                    fsJsonPrinter.CompressedJson(data));

                Debug.Log("Save file created." + Application.persistentDataPath);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return Task.CompletedTask;
        }

        public static Task Load(bool test = false)
        {
            try
            {
                var filePath = Application.persistentDataPath + $"/{NoobSaveData.Instance.saveFileName}.json";

                if (File.Exists(filePath))
                {
                    SaveData result = default;
                    var jsonData = File.ReadAllText(filePath);
                    var serializer = new fsSerializer();
                    fsData data;

                    if (NoobSaveData.Instance.usingEncrypt)
                    {
                        var decryptString = NoobSaveCrypter.DecryptJson(jsonData, NoobSaveData.Instance.encryptionKey);
                        data = fsJsonParser.Parse(decryptString);
                    }
                    else
                        data = fsJsonParser.Parse(jsonData);

                    serializer.TryDeserialize(data, ref result);
                    NoobSaveData.Instance.saveData = result;

                    foreach (var noobSaveStruct in result.saveStructs)
                        noobSaveStruct.GetValue();

                    if (!test)
                    {
                        var saveObjects = Object.FindObjectsOfType<MonoBehaviour>().OfType<INoobSaveable>();

                        foreach (var saveObject in saveObjects)
                            saveObject.Load(ref result);
                    }

                    Debug.Log("Save file loaded.");
                }
                else
                {
                    Debug.LogWarning("Save file not found.");
                    Save();
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
                throw;
            }

            return Task.CompletedTask;
        }

        public static void SetSave(string key, object obj)
        {
            var containsSave = ContainsSave(key);
            
            if (containsSave.Item1)
            {
                Debug.LogWarning($"NoobSave: {key} already exists in save data so it will be replaced.");
                NoobSaveData.Instance.saveData.saveStructs[containsSave.Item2] = new NoobSaveStruct(key, obj);
                return;
            }

            NoobSaveData.Instance.saveData.saveStructs.Add(new NoobSaveStruct(key, obj));
        }

        public static void RemoveSave(string key)
        {
            var findValue = 
                (NoobSaveData.Instance.saveData.saveStructs.FirstOrDefault(x => x.saveID == key));

            if (findValue.Equals(default(NoobSaveStruct)))
            {
                Debug.LogError($"NoobSave: {key} not found in save data.");
                return;
            }

            NoobSaveData.Instance.saveData.saveStructs.Remove(findValue);
        }

        public static void ClearSave()
        {
            NoobSaveData.Instance.saveData.saveStructs.Clear();
        }

        public static (bool,int) ContainsSave(string key)
        {
            var index = -1;
            var findValue = NoobSaveData.Instance.saveData.saveStructs.FirstOrDefault(x => x.saveID == key);
            if(!findValue.Equals(default(NoobSaveStruct)))
                index = NoobSaveData.Instance.saveData.saveStructs.IndexOf(findValue);
            
            return (index != -1,index);
        }

        public static T GetSave<T>(string key)
        {
            if (NoobSaveData.Instance.saveData.saveStructs.Count == 0)
            {
                Debug.LogWarning("NoobSave: Save data is empty.");
                return default;
            }
            
            var findValue = NoobSaveData.Instance.saveData.saveStructs.FirstOrDefault(x => x.saveID == key).obj;

            if (findValue != null) return (T)findValue;

            Debug.LogWarning($"NoobSave: {key} not found in save data.");
            return default;
        }
        
        public static int GetIntSave(string key)
        {
           return GetSave<int>(key);
        }
        
        public static float GetFloatSave(string key)
        {
            return GetSave<float>(key);
        }
        
        public static string GetStringSave(string key)
        {
            return GetSave<string>(key);
        }
    }
}
