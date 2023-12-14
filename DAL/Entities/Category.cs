using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace DAL
{
    public class CategoryCreate
    {
        [XmlElement]
        public string Name { get; set; }
        [XmlElement]
        public string[] Fields { get; set; }
        public CategoryCreate() { }
        public CategoryCreate(string name, string[]? fields)
        {
            (Name, Fields) = (name, fields ?? []);
        }
        public override string ToString() => Name;
    }
    [Serializable]
    public class Category: CategoryCreate
    {
        [XmlElement]
        public double Id { get; set; }
        public Category() { }
        [JsonConstructor]
        public Category(double id, string name, string[]? fields):base(name, fields)
        {
            (Id) = (id);
        }
    }
}
