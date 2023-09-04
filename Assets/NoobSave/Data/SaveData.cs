using System;
using System.Collections.Generic;
using System.Linq;
using EasyButtons;
using UnityEngine;

namespace NoobSave
{
    [Serializable]
    public partial class SaveData
    {
        [SerializeField] 
        internal List<NoobSaveStruct> saveStructs = new();
        
        [Button]
        public void ClearData()
        {
            saveStructs.Clear();
        }
        
        public T GetSave<T>(string key)
        {
            foreach (var saveStruct in saveStructs.Where(saveStruct => saveStruct.saveID == key))
            {
                return (T) saveStruct.obj;
            }
            
            Debug.LogError("Save not found with key: " + key);
            return default;
        }
    }
}