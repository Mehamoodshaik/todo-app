
using TodoApp.Models;
using TodoApp.Interfaces;

public class InMemoryTodoProvider : ITodoProvider
{
    private readonly List<Todo> _todos = new();
    private int _nextId = 1;

    public Task<List<Todo>> GetTodos(string search)
    {
        var result = string.IsNullOrWhiteSpace(search)
            ? _todos
            : _todos.Where(t => t.Title.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
        return Task.FromResult(result);
    }

    public Task<Todo> AddTodo(Todo todo)
    {
        todo.Id = _nextId++;
        todo.CreatedAt = DateTime.UtcNow;
        _todos.Add(todo);
        return Task.FromResult(todo);
    }

    public Task<Todo> UpdateTodo(Todo todo)
    {
        var index = _todos.FindIndex(t => t.Id == todo.Id);
        if (index >= 0)
        {
            todo.UpdatedAt = DateTime.UtcNow;
            _todos[index] = todo;
        }
        return Task.FromResult(todo);
    }

    public Task<bool> DeleteTodo(int id)
    {
        var removed = _todos.RemoveAll(t => t.Id == id) > 0;
        return Task.FromResult(removed);
    }
}
