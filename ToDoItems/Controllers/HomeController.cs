using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using ToDoItems.Models;

namespace ToDoItems.Controllers
{
    public class HomeController : Controller
    {
        private static string _connectionString = @"Data Source=.\sqlexpress;Initial Catalog=ToDoItems;Integrated Security=true;TrustServerCertificate=yes;";
        ToDoItemManager itemManager = new ToDoItemManager(_connectionString);

        public IActionResult Index()
        {
            IndexViewModel vm = new IndexViewModel
            {
                items = itemManager.GetUncompletedItems()
            };
            return View(vm);
        }

        public IActionResult Categories()
        {
            CategoriesViewModel cv = new CategoriesViewModel
            {
                categories = itemManager.GetCategories()
            };
            return View(cv);
        }

        public IActionResult NewCategory()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SaveNewCategory(string name)
        {
            itemManager.AddCategory(name);
            return Redirect("/home/index");
        }

        public IActionResult EditCategory(int id)
        {
            EditCategoryViewModel ecv = new EditCategoryViewModel
            {
                Category = itemManager.GetCategoryById(id)
            };
            return View(ecv);
        }

        [HttpPost]
        public IActionResult SaveEditedCategory(Category c)
        {
            itemManager.UpdateCategory(c);
            return Redirect("/home/categories");
        }

        public IActionResult NewItem()
        {
            NewItemViewModel niv = new NewItemViewModel
            {
                categories = itemManager.GetCategories()
            };
            return View(niv);
        }

        [HttpPost]
        public IActionResult SaveNewItem(string title, DateTime dueDate, int categoryId)
        {
            itemManager.AddItem(title, dueDate, categoryId);
            return Redirect("/home/index");
        }

        [HttpPost]
        public IActionResult MarkAsCompleted(int id)
        {
            itemManager.MarkAsCompleted(id);
            return Redirect("/Home/Index");
        }

        public IActionResult Completed()
        {
            IndexViewModel iv = new IndexViewModel
            {
                items = itemManager.GetCompletedItems()
            };
            return View(iv);
        }

        public IActionResult ItemsForCategory(int id)
        {
            ItemsForCatViewModel ifcv = new ItemsForCatViewModel
            {
                items = itemManager.GetItemsForCategory(id)
            };
            return View(ifcv);
        }
    }
}
