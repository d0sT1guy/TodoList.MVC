using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using Todo.Models;
using Todo.Models.ViewModels;

namespace Todo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index() 
        {
            var todoListViewModel = GetAllTodos();
            return View(todoListViewModel);
        }

        [HttpPost]

        public IActionResult PopulateForm(int id) //taking only ONE todo for updating form
        {
            var todo = GetById(id); //takes
            return Json(todo);  //returns json form with data
        }
        
        [HttpGet]
        internal TodoViewModel GetAllTodos() // view all db on the page
        {
            List<TodoItem> todoList = new();    

            using (SqliteConnection con = new SqliteConnection("DataSource=db.sqlite"))
            {
                using (var tableCmd = con.CreateCommand())
                {
                    con.Open();
                    tableCmd.CommandText = $"SELECT * FROM todo ";

                    using (var reader = tableCmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                todoList.Add(
                                    new TodoItem
                                    {
                                        Id = reader.GetInt32(0),
                                        Name = reader.GetString(1)
                                    });
                            }
                        }
                        else
                        {
                            return new TodoViewModel
                            {
                                TodoList = todoList
                            };
                        }
                    };
                }
            }
            return new TodoViewModel
                {
                    TodoList = todoList
                };
        }


        [HttpGet]
        internal TodoItem GetById(int id)
        {
            TodoItem todo = new();

            using (var connection = new SqliteConnection("Data Source=db.sqlite"))
            {
                using (var tableCmd = connection.CreateCommand())
                {
                    connection.Open();
                    tableCmd.CommandText = $"SELECT * FROM todo WHERE Id = '{id}'";

                    using (var reader = tableCmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            todo.Id = reader.GetInt32(0);
                            todo.Name = reader.GetString(1);
                        }
                        else
                        {
                            return todo;
                        }
                    };
                }
            }

            return todo;
        }

        [HttpPost]
        public IActionResult Insert(TodoItem todo)
        {
            using (SqliteConnection con = new SqliteConnection("DataSource=db.sqlite"))
            {
                using (var tableCmd = con.CreateCommand())
                {
                    con.Open();
                    tableCmd.CommandText = $"INSERT INTO todo (name) VALUES ('{todo.Name}')";
                    try
                    {
                        tableCmd.ExecuteNonQuery();
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        return Redirect("https://localhost:5001/");
        }

        [HttpPost]

        public IActionResult Delete(int id)
        {
            using (SqliteConnection con = new SqliteConnection("Data Source=db.sqlite"))
            {
                using (var tableCmd = con.CreateCommand())
                {
                    con.Open();
                    tableCmd.CommandText = $"DELETE FROM todo WHERE Id = '{id}'";
                    tableCmd.ExecuteNonQuery();
                }
            }

            return Json(new {});
        }


        [HttpPost]
        public IActionResult Update(TodoItem todo)
        {
            using (SqliteConnection con = new SqliteConnection("Data Source=db.sqlite"))
            {
                using (var tableCmd = con.CreateCommand())
                {
                    con.Open();
                    tableCmd.CommandText = $"UPDATE todo SET name = '{todo.Name}' WHERE Id = '{todo.Id}'";
                    try
                    {
                        tableCmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }

            return Redirect("https://localhost:5001/");
        }
    }
}
