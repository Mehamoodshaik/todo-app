import axios from 'axios'

const apiUrl = 'https://localhost:52118/api/todo'

export const ApiProvider = {
  async getTodos() {
    const response = await axios.get(apiUrl)
    return response.data
  },
  async addTodo(todo) {
    await axios.post(apiUrl, todo)
  },
  async updateTodo(todo) {
    await axios.put(apiUrl, todo)
  },
  async deleteTodo(id) {
    await axios.delete(`${apiUrl}/${id}`)
  }
}
