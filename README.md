
# NoobSave

Noobsave is an encryptable, crossplatform simple unity save system.




## Example

```c#
namespace NoobSave.Example
{
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
}
```


## Main Funcs

```c#
        public static void AddSave(string key, object obj) //Add save with key
        {
            NoobSaveData.Instance.saveData.saveStructs.Add(new NoobSaveStruct(key, obj));
        }

        public static void RemoveSave(string key) //Remove save data with key
        {
            NoobSaveData.Instance.saveData.saveStructs.RemoveAll(x => x.saveID == key);
        }

        public static void ClearSave()
        {
            NoobSaveData.Instance.saveData.saveStructs.Clear();
        }

        public static bool ContainsSave(string key) //Return true if data matches the given key, false otherwise
        {
            return NoobSaveData.Instance.saveData.saveStructs.Any(x => x.saveID == key);
        }

        public static T GetSave<T>(string key) //Get Data by key
        {
            return (T)NoobSaveData.Instance.saveData.saveStructs.Find(x => x.saveID == key).obj;
        }
```

  
## NoobSave Data
Here you can see the data and other settings
![Uygulama Ekran Görüntüsü](https://i.imgur.com/fWQhcvN.png)

### And we have a menu
![Uygulama Ekran Görüntüsü](https://i.imgur.com/nluaoCH.png)
