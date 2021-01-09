using FiltersTest.Models;
using FiltersTest.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FiltersTest.Controllers
{
    [ApiController]
    public class TodosController : ControllerBase
    {
        private readonly ITodoService todoService;

        public TodosController(ITodoService todoService) => this.todoService = todoService;

        [HttpPost("/todos")]
        public async Task<IActionResult> AddTodo(Todo todo)
        {
            throw new Exception("This is atest");
            var id = await todoService.Add(todo);
            return Ok(id);
        }

        [HttpDelete("/todos/{id}")]
        public async Task<IActionResult> DeleteTodo(int id)
        {
            await todoService.Delete(id);
            return Ok();
        }
    }
}
