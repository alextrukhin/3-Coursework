using System.Text.RegularExpressions;

namespace DAL
{
    public class EntityContext<T> : IEntityContext<T> where T : class, new()
    {
        public IProvider<T> Provider { get; set; }
        string _DBName = "custom-db";
        public string DBName { get => _DBName; set { _DBName = value ?? throw new ArgumentException(); } }
        public string DBType { get; set; }
        public string DBFile => $"{DBName}.{DBType}";
        public string[] AvailableDBTypes => new string[] { "json", "xml" };
        public EntityContext()
        {
            DBType = "json";
            Provider = new JSONProvider<T>(DBFile);
        }
        public EntityContext(string dbType, string dbName)
        {
            SetProvider(dbType, dbName);
        }
        public void SetProvider(string dbType, string dbName)
        {
            DBType = dbType;
            DBName = dbName;
            switch (dbType)
            {
                case "json":
                    Provider = new JSONProvider<T>(DBFile);
                    break;
                case "xml":
                    Provider = new XMLProvider<T>(DBFile);
                    break;
            }
        }
        public void SetProvider(string FileName)
        {
            if (!Regex.IsMatch(FileName, @"^[a-zA-Z0-9_\-\.]+\.[a-zA-Z0-9]+$")) throw new ArgumentException();
            string[] Parts = FileName.Split('.');
            SetProvider(Parts[1], Parts[0]);
        }
    }
}
