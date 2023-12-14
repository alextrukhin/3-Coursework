using DAL;

namespace BLL
{
    public class BaseService<T> : IDisposable where T : class, new()
    {
        protected List<T> data = [];
        public IEntityContext<T> db = new EntityContext<T>();
        public BaseService()
        {
            data = db.Provider.Load();
        }
        public BaseService(IEntityContext<T> customDB)
        {
            db = customDB;
            data = db.Provider.Load();
        }
        public void Insert(T input)
        {
            data.Add(input);
            db.Provider.Save(data);
        }
        public void Update(T input, int index)
        {
            data[index] = input;
            db.Provider.Save(data);
        }
        public void UpdateById(T input, Predicate<T> SearchFunction)
        {
            data[data.FindIndex(SearchFunction)] = input;
            db.Provider.Save(data);
        }
        public T? GetById(Predicate<T> SearchFunction)
        {
            return data.Find(SearchFunction);
        }
        public void DeleteById(Predicate<T> SearchFunction)
        {
            data.RemoveAt(data.FindIndex(SearchFunction));
            db.Provider.Save(data);
        }
        public void Delete(int index)
        {
            data.RemoveAt(index);
            db.Provider.Save(data);
        }
        public List<T> Sort(Comparison<T> comparison)
        {
            List<T> sorted = new (data);
            sorted.Sort(comparison);
            return sorted;
        }
        public List<T> Data => data;
        public void Load() => db.Provider.Load();
        public void Save() => db.Provider.Save(data);
        public int Length() => data.Count;
        public string DBName => db.DBName;
        public string DBType => db.DBType;
        public string DBFile => db.DBFile;
        public void SetProvider(string dbType, string dbName) { db.SetProvider(dbType, dbName); data = db.Provider.Load(); }
        public void SetProvider(string FileName) { db.SetProvider(FileName); data = db.Provider.Load(); }
        public string[] AvailableDBTypes => db.AvailableDBTypes;
        public List<Tuple<int, T>> Search(Func<T, bool> filterFunction)
        {
            List<Tuple<int, T>> Entities = new();
            if (data.Count == 0) { return Entities; }
            for (int i = 0; i < data.Count; i++)
            {
                T cur = data[i];
                if (filterFunction(cur)) Entities.Add(Tuple.Create(i, cur));
            }
            return Entities;
        }
        public T this[int position]
        {
            get => data[position];
            set => data[position] = value;
        }
        public void Dispose()
        {
        }
    }
}
