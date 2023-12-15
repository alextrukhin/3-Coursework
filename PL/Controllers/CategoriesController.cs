using BLL;
using DAL;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace PL.Controllers
{
    public class CategoriesController : Controller
    {
        public ActionResult Index(string Id="Name")
        {
            using CategoryService categoryService = new(HttpContext.Session.GetString("db_type") ?? "json");
            switch (Id)
            {
                default:
                    return View(categoryService.Sort(CategoryService.SortByNameComparator));
            }
        }
        public ActionResult View(double? Id)
        {
            using ProductService products = new(HttpContext.Session.GetString("db_type") ?? "json");
            using CategoryService categories = new(HttpContext.Session.GetString("db_type") ?? "json");
            Category? category = categories.GetById(el => el.Id == Id);
            return View(new ViewCategoryViewModel { Category = category, Products = products.FindByCategoryId(Id) });
        }
        public ActionResult Create()
        {
            List<string> listOfFieldNames = typeof(Product).GetProperties().Where(f => f.CanWrite).Select(f => f.Name).Where(f=>!ProductService.IgnoreFields.Contains(f)).ToList();
            return View(new CreateViewModel {Fields=listOfFieldNames});
        }
        [HttpPost]
        public ActionResult Create(CategoryCreate createValues)
        {
            using CategoryService categoryService = new(HttpContext.Session.GetString("db_type") ?? "json");
            categoryService.Insert(createValues);
            return RedirectToAction(nameof(Index));
        }
        public ActionResult Edit(double Id)
        {
            List<string> listOfFieldNames = typeof(Product).GetProperties().Where(f => f.CanWrite).Select(f => f.Name).Where(f => !ProductService.IgnoreFields.Contains(f)).ToList();
            using CategoryService categoryService = new(HttpContext.Session.GetString("db_type") ?? "json");

            Category? category = categoryService.GetById((el) => el.Id == Id);
            return View(new EditViewModel { Fields = listOfFieldNames, Category = category, PreSelectedFields = new List<string>(category != null ? category.Fields : []) });
        }
        [HttpPost]
        public ActionResult Edit(Category newValues)
        {
            using CategoryService categoryService = new(HttpContext.Session.GetString("db_type") ?? "json");
            categoryService.UpdateById(newValues, (el)=>el.Id==newValues.Id);
            return RedirectToAction(nameof(Index));
        }
        public ActionResult Delete(double Id)
        {
            using CategoryService categoryService = new(HttpContext.Session.GetString("db_type") ?? "json");
            categoryService.DeleteById(Id);
            return RedirectToAction(nameof(Index));
        }
    }
    public class ViewCategoryViewModel
    {
        public Category? Category { get; set; }
        public List<Product> Products { get; set; }
    }
    public class CreateViewModel
    {
        public List<string> Fields { get; set; }
    }
    public class EditViewModel: CreateViewModel
    {
        public Category? Category { get; set; }
        public List<string> PreSelectedFields { get; set; }
    }
}
