using TodoApp.Interfaces;
using TodoApp.Providers;  
using TodoApp.Data;       
namespace TodoApp.Factories
{
    public class TodoProviderFactory
    {
        public static ITodoProvider CreateProvider(string type, AppDbContext context)
        {
            return type switch
            {
                "mysql" => new MySqlTodoProvider(context),
                "memory" => new InMemoryTodoProvider(),
                _ => throw new ArgumentException("Invalid provider type")
            };
        }
    }
}
