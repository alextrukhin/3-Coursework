namespace DAL
{
    public interface IProvider<T> where T : class
    {
        public List<T> Load();
        public void Save(List<T> listToSave);
    }
    public interface IEntityContext<T> where T : class, new()
    {
        public IProvider<T> Provider { get; set; }
        public string DBName { get; set; }
        public string DBType { get; set; }
        public string DBFile { get; }
        public string[] AvailableDBTypes { get; }
        public void SetProvider(string dbType, string dbName)
        {
        }
        public void SetProvider(string FileName)
        {
        }
    }
}