
using Microsoft.AspNetCore.Mvc;
using TodoApp.Models;
using TodoApp.Factories;
using TodoApp.Data;


namespace TodoApp.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly AppDbContext _context;
        public TodoController(AppDbContext context) => _context = context;

        [HttpGet]
        public async Task<IActionResult> Get(string search = null, string provider = "mysql")
        {
            var todoProvider = TodoProviderFactory.CreateProvider(provider, _context);
            return Ok(await todoProvider.GetTodos(search));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Todo todo, [FromQuery] string provider = "mysql")
        {
            var todoProvider = TodoProviderFactory.CreateProvider(provider, _context);
            return Ok(await todoProvider.AddTodo(todo));
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Todo todo, [FromQuery] string provider = "mysql")
        {
            var todoProvider = TodoProviderFactory.CreateProvider(provider, _context);
            return Ok(await todoProvider.UpdateTodo(todo));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, [FromQuery] string provider = "mysql")
        {
            var todoProvider = TodoProviderFactory.CreateProvider(provider, _context);
            return Ok(await todoProvider.DeleteTodo(id));
        }
    }

}