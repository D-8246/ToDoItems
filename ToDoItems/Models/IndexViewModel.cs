using Microsoft.AspNetCore.Mvc.Formatters;

namespace ToDoItems.Models
{
    public class IndexViewModel
    {
        public List<Item> items { get; set; } = new List<Item>();
    }
}
