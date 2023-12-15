using BLL;
using DAL;
using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class SuppliersController : Controller
    {
        public ActionResult Index([FromQuery(Name = "sort")] string sorting = "Id")
        {
            using SupplierService suppliers = new(HttpContext.Session.GetString("db_type") ?? "json");
            switch (sorting)
            {
                case "Name":
                    return View(suppliers.Sort(SupplierService.SortByNameComparator));
                case "LastName":
                    return View(suppliers.Sort(SupplierService.SortByLastNameComparator));
                default:
                    return View(suppliers.Data);
            }
        }
        public ActionResult View(double id)
        {
            using ProductService products = new(HttpContext.Session.GetString("db_type") ?? "json");
            using SupplierService suppliers = new(HttpContext.Session.GetString("db_type") ?? "json");
            Supplier? supplier = suppliers.GetById(el => el.Id == id);
            List<Product> productsOfSuppliers = products.FindByIds(supplier.ProductIds);
            return View(new ViewSupplierViewModel { Supplier = supplier, Products = productsOfSuppliers });
        }
        public ActionResult Create()
        {
            return View(new CreateSupplierViewModel {  });
        }
        [HttpPost]
        public ActionResult Create(SupplierCreate createValues)
        {
            using SupplierService suppliers = new(HttpContext.Session.GetString("db_type") ?? "json");
            suppliers.Insert(createValues);
            return RedirectToAction(nameof(Index));
        }
        public ActionResult Edit(double Id)
        {
            using SupplierService suppliers = new(HttpContext.Session.GetString("db_type") ?? "json");
            Supplier supplier = suppliers.GetById((el) => el.Id == Id);
            return View(new EditSupplierViewModel { Supplier = supplier });
        }
        [HttpPost]
        public ActionResult Edit(Supplier newValues)
        {
            using SupplierService suppliers = new(HttpContext.Session.GetString("db_type") ?? "json");
            suppliers.UpdateById(newValues, (el) => el.Id == newValues.Id);
            return RedirectToAction(nameof(Index));
        }
        public ActionResult Delete(double Id)
        {
            using SupplierService suppliers = new(HttpContext.Session.GetString("db_type") ?? "json");
            suppliers.DeleteById((el) => el.Id == Id);
            return RedirectToAction(nameof(Index));
        }
    }
    public class ViewSupplierViewModel
    {
        public Supplier Supplier { get; set; }
        public List<Product> Products { get; set; }
    }
    public class CreateSupplierViewModel
    {
    }
    public class EditSupplierViewModel : CreateSupplierViewModel
    {
        public Supplier Supplier { get; set; }
    }
}
