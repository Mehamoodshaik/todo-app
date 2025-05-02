const STORAGE_KEY = 'local-todos'

export const LocalProvider = {
  async getTodos() {
    const todos = JSON.parse(localStorage.getItem(STORAGE_KEY)) || []
    return todos
  },
  async addTodo(todo) {
    const todos = await this.getTodos()
    todo.id = Date.now()
    todos.push(todo)
    localStorage.setItem(STORAGE_KEY, JSON.stringify(todos))
  },
  async updateTodo(updatedTodo) {
    const todos = await this.getTodos()
    const index = todos.findIndex(t => t.id === updatedTodo.id)
    if (index !== -1) {
      todos[index] = updatedTodo
      localStorage.setItem(STORAGE_KEY, JSON.stringify(todos))
    }
  },
  async deleteTodo(id) {
    let todos = await this.getTodos()
    todos = todos.filter(t => t.id !== id)
    localStorage.setItem(STORAGE_KEY, JSON.stringify(todos))
  }
}
