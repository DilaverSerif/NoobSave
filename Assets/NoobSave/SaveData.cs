using System;
using System.Collections.Generic;
using UnityEngine;

namespace NoobSave
{
    [Serializable]
    public partial class SaveData
    {
        [SerializeField] 
        internal List<NoobSaveStruct> saveStructs = new();
        public void ClearData()
        {
            saveStructs.Clear();
        }
    }
}