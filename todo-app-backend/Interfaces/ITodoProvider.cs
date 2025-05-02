using TodoApp.Models;

namespace TodoApp.Interfaces
{
    public interface ITodoProvider
    {
        Task<List<Todo>> GetTodos(string search = null);
        Task<Todo> AddTodo(Todo todo);
        Task<Todo> UpdateTodo(Todo todo);
        Task<bool> DeleteTodo(int id);
    }
}
