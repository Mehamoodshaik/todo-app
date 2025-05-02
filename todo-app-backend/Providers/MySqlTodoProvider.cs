using TodoApp.Models;
using TodoApp.Interfaces;
using TodoApp.Data; 
using Microsoft.EntityFrameworkCore; 

namespace TodoApp.Providers
{
    public class MySqlTodoProvider : ITodoProvider
    {
        private readonly AppDbContext _context;
        public MySqlTodoProvider(AppDbContext context) => _context = context;

        public async Task<List<Todo>> GetTodos(string search)
        {
            return await _context.Todos
                .Where(t => string.IsNullOrEmpty(search) || t.Title.Contains(search))
                .ToListAsync();
        }

        public async Task<Todo> AddTodo(Todo todo)
        {
            todo.CreatedAt = DateTime.UtcNow;
            _context.Todos.Add(todo);
            await _context.SaveChangesAsync();
            return todo;
        }

        public async Task<Todo> UpdateTodo(Todo todo)
        {
            todo.UpdatedAt = DateTime.UtcNow;
            _context.Todos.Update(todo);
            await _context.SaveChangesAsync();
            return todo;
        }

        public async Task<bool> DeleteTodo(int id)
        {
            var todo = await _context.Todos.FindAsync(id);
            if (todo == null) return false;
            _context.Todos.Remove(todo);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
