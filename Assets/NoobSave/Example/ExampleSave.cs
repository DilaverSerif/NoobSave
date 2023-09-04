using System;
using NoobSave;
using NoobSave.Interfaces;
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

    private void Start()
    {
        NoobSaveMain.Load();
        Debug.Log( NoobSaveMain.GetSave<string>("NormalKeyTest"));
    }

    public void Save()
    {
        var gbSave = new GameObjectSave(transform.position,transform.rotation.eulerAngles);
        NoobSaveMain.AddSave("ExampleSave",gbSave);
        NoobSaveMain.AddSave("InterfaceKeyTest","Interface is working!");
        NoobSaveMain.AddSave("NormalKeyTest","Normal key is working!");
    }

    public void Load(ref SaveData saveData)
    {
        Debug.Log(saveData.GetSave<string>("InterfaceKeyTest"));
    }
    
}
