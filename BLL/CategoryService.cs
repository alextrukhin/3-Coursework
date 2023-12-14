using DAL;
using System.Reflection;

namespace BLL
{
    public class CategoryService : BaseService<Category>
    {
        public CategoryService(string type = "json") : base(new EntityContext<Category>(type, "categories")) { }
        public CategoryService(IEntityContext<Category> customDB) : base(customDB) { }
        public Category Insert(CategoryCreate input)
        {
            double LastId = 1;
            try
            {
                LastId = data[^1].Id + 1;
            }
            catch (Exception) { }
            Category newCategory = new(LastId, input.Name, input.Fields);
            data.Add(newCategory);
            db.Provider.Save(data);
            return newCategory;
        }
        public Category? FindById(double? Id)
        {
            return data.Find(el => el.Id == Id);
        }
        public static List<Tuple<string, string>> GetFields(Category? category)
        {
            List<Tuple<string, string>> fields = [];
            string[] list = category != null ? category.Fields : typeof(Product).GetProperties().Where(f => f.CanWrite).Select(f => f.Name).Where(f => !ProductService.IgnoreFields.Contains(f)).ToArray();
            foreach (var field in list)
            {
                PropertyInfo? myFieldInfo = typeof(Product).GetProperty(field);
                if (myFieldInfo != null)
                {
                    string name = myFieldInfo.PropertyType.Name;
                    if (myFieldInfo.PropertyType.IsGenericType &&
                         myFieldInfo.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        name = myFieldInfo.PropertyType.GetGenericArguments()[0].Name;
                    }
                    fields.Add(new Tuple<string, string>(field, name));
                }
            }
            return fields;
        }
        public Category? DeleteById(double? Id)
        {
            Category? category = data.Find(el => el.Id == Id);
            if (category != null)
            {
                data.Remove(category);
                ProductService products = new();
                products.CategoryRemoved(category.Id);
                db.Provider.Save(data);
            }
            return category;
        }
        public static Comparison<Category> SortByNameComparator => delegate (Category x, Category y)
            {
                if (x.Name == null && y.Name == null) return 0;
                else if (x.Name == null) return -1;
                else if (y.Name == null) return 1;
                else return x.Name.CompareTo(y.Name);
            };
    }
}
