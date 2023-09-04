using System;

namespace NoobSave
{
    [Serializable]
    public struct NoobSaveStruct
    {
        public string saveID;
        public string saveValue;
        public object obj;
    
        public NoobSaveStruct(string saveID, object obj)
        {
            this.saveID = saveID;
            this.obj = obj;
            saveValue = obj.ToString();
        }
        
        public void SetValue(object obj)
        {
            this.obj = obj;
            saveValue = obj.ToString();
        }
        
        public void GetValue()
        {
            saveValue = obj.ToString();
        }
    }
}