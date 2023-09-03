namespace NoobSave
{
    public interface INoobSaveable
    {
        public void Save();
        public void Load(ref SaveData saveData);
    }
}