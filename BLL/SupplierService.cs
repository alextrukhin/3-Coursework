using DAL;

namespace BLL
{
    public class SupplierService : BaseService<Supplier>
    {
        public SupplierService(string type = "json") : base(new EntityContext<Supplier>(type, "suppliers")) { }
        public SupplierService(IEntityContext<Supplier> customDB) : base(customDB) { }
        public Supplier Insert(SupplierCreate input)
        {
            double LastId = 1;
            try
            {
                LastId = data[^1].Id + 1;
            }
            catch (Exception) { }
            Supplier supplier = new(input, LastId);
            data.Add(supplier);
            db.Provider.Save(data);
            return supplier;
        }
        public Supplier? FindById(double Id)
        {
            return data.Find(el => el.Id == Id);
        }
        public Supplier? UpdateById(Supplier input, double Id)
        {
            int pos = data.FindIndex(el => el.Id == Id);
            if (pos == -1)
            {
                return null;
            }
            data[pos] = input;
            db.Provider.Save(data);
            return input;
        }
        public void ProductRemoved(double ProductId)
        {
            foreach (Supplier supplier in data.Where(i => new List<double>(i.ProductIds).Contains(ProductId)))
            {
                supplier.ProductIds = new List<double>(supplier.ProductIds).Where(e => e != ProductId).ToArray();
            }
            db.Provider.Save(data);
        }
        public List<Supplier> HaveProduct(double? ProductId)
        {
            if (ProductId == null) { return new List<Supplier>([]); }
            return new List<Supplier>(data.Where(i => new List<double>(i.ProductIds ?? []).Contains(ProductId ?? 0)));
        }
        public List<Supplier> Search(string? query)
        {
            if (query == null) { return new List<Supplier>([]); }
            return new List<Supplier>(data.Where(supplier =>
            Utils.Contains(supplier.Name ?? "", query, StringComparison.OrdinalIgnoreCase) == true
            || Utils.Contains(supplier.LastName ?? "", query, StringComparison.OrdinalIgnoreCase) == true
            || Utils.Contains(supplier.Description ?? "", query, StringComparison.OrdinalIgnoreCase) == true
            || Utils.Contains(supplier.Email ?? "", query, StringComparison.OrdinalIgnoreCase) == true
            || Utils.Contains(supplier.Phone ?? "", query, StringComparison.OrdinalIgnoreCase) == true
            || Utils.Contains(supplier.Address ?? "", query, StringComparison.OrdinalIgnoreCase) == true
            ));
        }
        public static Comparison<Supplier> SortByNameComparator => delegate (Supplier x, Supplier y)
        {
            if (x.Name == null && y.Name == null) return 0;
            else if (x.Name == null) return -1;
            else if (y.Name == null) return 1;
            else return x.Name.CompareTo(y.Name);
        };
        public static Comparison<Supplier> SortByLastNameComparator => delegate (Supplier x, Supplier y)
        {
            if (x.LastName == null && y.LastName == null) return 0;
            else if (x.LastName == null) return -1;
            else if (y.LastName == null) return 1;
            else return x.LastName.CompareTo(y.LastName);
        };
    }
}
