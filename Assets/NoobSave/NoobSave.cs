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
                    File.WriteAllText(Application.persistentDataPath + "/save.json", encryptString);
                    return Task.CompletedTask;
                }

                File.WriteAllText(Application.persistentDataPath + "/save.json", fsJsonPrinter.CompressedJson(data));

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
                var filePath = Application.persistentDataPath + "/save.json";

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

        public static void AddSave(string key, object obj)
        {
            NoobSaveData.Instance.saveData.saveStructs.Add(new NoobSaveStruct(key, obj));
        }

        public static void RemoveSave(string key)
        {
            NoobSaveData.Instance.saveData.saveStructs.RemoveAll(x => x.saveID == key);
        }

        public static void ClearSave()
        {
            NoobSaveData.Instance.saveData.saveStructs.Clear();
        }

        public static bool ContainsSave(string key)
        {
            return NoobSaveData.Instance.saveData.saveStructs.Any(x => x.saveID == key);
        }

        public static T GetSave<T>(string key)
        {
            return (T)NoobSaveData.Instance.saveData.saveStructs.Find(x => x.saveID == key).obj;
        }
    }
}