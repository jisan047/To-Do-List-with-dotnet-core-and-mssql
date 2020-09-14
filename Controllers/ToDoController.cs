using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoList.Infrastructure;
using ToDoList.Models;

namespace ToDoList.Controllers{
    public class ToDoController: Controller{
        private readonly ToDoContext context;
        public ToDoController(ToDoContext context){
            this.context = context;
            
        }
        public async Task<ActionResult> Index(){
            IQueryable<TodoList> items = from i in context.ToDoLists  orderby i.IsChecked select i;
            List<TodoList> todoLists = await items.ToListAsync();
            return View(todoLists);
        }

        [HttpPost]
        public async Task<ActionResult> Create(string Title){
            //Console.WriteLine(Title);
            TodoList item = new TodoList();
            item.Title = Title;
            item.IsChecked = false;
            context.Add(item);
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Edit(int Id){
            //Console.WriteLine(Id);
            TodoList item = await context.ToDoLists.FindAsync(Id);
            item.IsChecked = !item.IsChecked;
            context.Update(item);
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Delete(int Id){
            //Console.WriteLine(Id);
            TodoList item = await context.ToDoLists.FindAsync(Id);
            context.Remove(item);
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}