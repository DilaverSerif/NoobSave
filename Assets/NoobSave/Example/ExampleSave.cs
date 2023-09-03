using System;
using NoobSave;
using UnityEngine;

public class ExampleSave : MonoBehaviour,INoobSaveable
{
    [Serializable]
    public struct GameObjectSave
    {
        public Vector3 position;
        public Vector3 rotation;
        
        public GameObjectSave(Vector3 position, Vector3 rotation)
        {
            this.position = position;
            this.rotation = rotation;
        }
    }
    
    public void Save()
    {
        var gbSave = new GameObjectSave(transform.position,transform.rotation.eulerAngles);
        NoobSaveMain.AddSave("ExampleSave",gbSave);
        NoobSaveMain.AddSave("Berdirhan","Test");
    }

    public void Load(ref SaveData saveData)
    {
        Debug.Log(saveData.saveStructs[1].obj.ToString());
    }
    
}
