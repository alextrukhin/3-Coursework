using DAL;

namespace BLL
{
    public class ProductService : BaseService<Product>
    {
        public static readonly List<string> IgnoreFields = new(["Id", "CategoryId", "Name", "Brand", "InStock", "Description", "Price"]);
        public ProductService(string type = "json") : base(new EntityContext<Product>(type, "products")) { }
        public ProductService(IEntityContext<Product> customDB) : base(customDB) { }
        public Product Insert(ProductCreate input)
        {
            double LastId = 1;
            try
            {
                LastId = (data[^1].Id ?? 0) + 1;
            }
            catch (Exception) { }
            Product product = new(input, LastId);
            data.Add(product);
            db.Provider.Save(data);
            return product;
        }
        public List<Product> FindByCategoryId(double? CategoryId)
        {
            return data.FindAll(el => el.CategoryId == CategoryId);
        }
        public Product? FindById(double Id)
        {
            return data.Find(el => el.Id == Id);
        }
        public List<Product> FindByIds(double[] Ids)
        {
            List<double> ids = new(Ids ?? []);
            return new List<Product>(data.Where(product => ids.Contains(product.Id ?? 0)));
        }
        public void CategoryRemoved(double CategoryId)
        {
            foreach (Product product in data.Where(i => i.CategoryId == CategoryId))
            {
                product.CategoryId = null;
            }
            db.Provider.Save(data);
        }
        public List<Product> Search(string? query)
        {
            if (query == null) { return new List<Product>([]); }
            return new List<Product>(data.Where(product =>
            Utils.Contains(product.Name ?? "", query, StringComparison.OrdinalIgnoreCase) == true
            || Utils.Contains(product.Brand ?? "", query, StringComparison.OrdinalIgnoreCase) == true
            || Utils.Contains(product.Description ?? "", query, StringComparison.OrdinalIgnoreCase) == true
            ));
        }
        public static Comparison<Product> SortByNameComparator => delegate (Product x, Product y)
        {
            if (x.Name == null && y.Name == null) return 0;
            else if (x.Name == null) return -1;
            else if (y.Name == null) return 1;
            else return x.Name.CompareTo(y.Name);
        };
        public static Comparison<Product> SortByBrandComparator => delegate (Product x, Product y)
        {
            if (x.Brand == null && y.Brand == null) return 0;
            else if (x.Brand == null) return -1;
            else if (y.Brand == null) return 1;
            else return x.Brand.CompareTo(y.Brand);
        };
        public static Comparison<Product> SortByPriceComparator => delegate (Product x, Product y)
        {
            if (x.Price == null && y.Price == null) return 0;
            else if (x.Price == null) return -1;
            else if (y.Price == null) return 1;
            else return x.Price.Value.CompareTo(y.Price.Value);
        };
    }
}
