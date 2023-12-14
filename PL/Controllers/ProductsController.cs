using BLL;
using DAL;
using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class ProductsController : Controller
    {
        public ActionResult Index([FromQuery(Name = "sort")] string sorting = "Id")
        {
            using ProductService products = new(HttpContext.Session.GetString("db_type") ?? "json");
            switch (sorting)
            {
                case "Name":
                    return View(products.Sort(ProductService.SortByNameComparator));
                case "Price":
                    return View(products.Sort(ProductService.SortByPriceComparator));
                case "Brand":
                    return View(products.Sort(ProductService.SortByBrandComparator));
                default:
                    return View(products.Data);
            }
        }
        public ActionResult View(double id)
        {
            using ProductService products = new(HttpContext.Session.GetString("db_type") ?? "json");
            Product? product = products.FindById(id);
            Category? category = null;
            List<Tuple<string, string>> fields = CategoryService.GetFields(null).ConvertAll(field => new Tuple<string, string>(field.Item1, CSTypeNameToHTML(field.Item2)));
            if (product != null && product.CategoryId != null)
            {
                using CategoryService categories = new(HttpContext.Session.GetString("db_type") ?? "json");
                category = categories.FindById(product.CategoryId);
                fields = CategoryService.GetFields(category);
                fields = new List<Tuple<string, string>>(fields.Where(field => !ProductService.IgnoreFields.Contains(field.Item1)));
            }
            using SupplierService suppliers = new(HttpContext.Session.GetString("db_type") ?? "json");
            return View(new ViewProductViewModel { Product = product, Category = category, Fields = fields, Suppliers = suppliers.Data });
        }
        public ActionResult Create(double id)
        {
            using CategoryService categories = new(HttpContext.Session.GetString("db_type") ?? "json");
            Category? category = categories.GetById(el => el.Id == id);
            List<Tuple<string, string>> fields = CategoryService.GetFields(category).ConvertAll(field => new Tuple<string, string>(field.Item1, CSTypeNameToHTML(field.Item2)));
            fields = new List<Tuple<string, string>>(fields.Where(field => !ProductService.IgnoreFields.Contains(field.Item1)));
            return View(new CreateProductViewModel { Category = category, Fields = fields });
        }
        [HttpPost]
        public ActionResult Create(ProductCreate createValues)
        {
            using ProductService products = new(HttpContext.Session.GetString("db_type") ?? "json");
            products.Insert(createValues);
            return RedirectToAction(nameof(Index));
        }
        public ActionResult Edit(double Id)
        {
            using ProductService products = new(HttpContext.Session.GetString("db_type") ?? "json");
            using CategoryService categories = new(HttpContext.Session.GetString("db_type") ?? "json");
            Product? product = products.FindById(Id);
            Category? category = null;
            List<Tuple<string, string>> fields = CategoryService.GetFields(null).ConvertAll(field => new Tuple<string, string>(field.Item1, CSTypeNameToHTML(field.Item2)));
            if (product != null && product.CategoryId != null)
            {
                category = categories.FindById(product.CategoryId);
                fields = CategoryService.GetFields(category).ConvertAll(field => new Tuple<string, string>(field.Item1, CSTypeNameToHTML(field.Item2)));
                fields = new List<Tuple<string, string>>(fields.Where(field => !ProductService.IgnoreFields.Contains(field.Item1)));
            }
            return View(new EditProductViewModel { Fields = fields, Category = category, Categories = categories.Data, Product = product });
        }
        [HttpPost]
        public ActionResult Edit(Product newValues)
        {
            using ProductService products = new(HttpContext.Session.GetString("db_type") ?? "json");
            products.UpdateById(newValues, (el) => el.Id == newValues.Id);
            return RedirectToAction(nameof(Index));
        }
        public ActionResult Delete(double Id)
        {
            using ProductService products = new(HttpContext.Session.GetString("db_type") ?? "json");
            using SupplierService suppliers = new(HttpContext.Session.GetString("db_type") ?? "json");
            products.DeleteById((el) => el.Id == Id);
            suppliers.ProductRemoved(Id);
            return RedirectToAction(nameof(Index));
        }
        public ActionResult AddSupplier(double Id, double Id2)
        {
            using ProductService products = new(HttpContext.Session.GetString("db_type") ?? "json");
            using SupplierService suppliers = new(HttpContext.Session.GetString("db_type") ?? "json");

            Supplier? supplier = suppliers.FindById(Id2);
            if (supplier == null)
            {
                return RedirectToAction(nameof(View), new { Id = Id });
            }

            List<double> ids = new(supplier.ProductIds ?? new double[0])
            {
                Id
            };
            supplier.ProductIds = ids.ToArray();
            supplier = suppliers.UpdateById(supplier, Id2);

            return RedirectToAction(nameof(View), new { Id = Id });
        }
        public ActionResult DeleteSupplier(double Id, double Id2)
        {
            using ProductService products = new(HttpContext.Session.GetString("db_type") ?? "json");
            using SupplierService suppliers = new(HttpContext.Session.GetString("db_type") ?? "json");

            Supplier? supplier = suppliers.FindById(Id2);
            if (supplier == null)
            {
                return RedirectToAction(nameof(View), new { Id = Id });
            }

            supplier.ProductIds = new List<double>(supplier.ProductIds ?? new double[0]).Where(s => s != Id).ToArray();
            suppliers.UpdateById(supplier, Id2);

            return RedirectToAction(nameof(View), new { Id = Id });
        }
        public string CSTypeNameToHTML(string input)
        {
            return input switch
            {
                "Float" => "number",
                "Int32" => "number",
                _ => "text",
            };
        }
    }
    public class ViewProductViewModel
    {
        public Product? Product { get; set; }
        public Category? Category { get; set; }
        public List<Tuple<string, string>> Fields { get; set; }
        public List<Supplier> Suppliers { get; set; }
    }
    public class CreateProductViewModel
    {
        public Category? Category { get; set; }
        public List<Tuple<string, string>> Fields { get; set; }
    }
    public class EditProductViewModel : CreateProductViewModel
    {
        public Product? Product { get; set; }
        public List<Category> Categories { get; set; }
    }
}
