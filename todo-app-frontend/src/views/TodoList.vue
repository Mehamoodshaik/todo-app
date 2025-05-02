<template>
  <div class="container">
    <h1>Todo List</h1>

    <div class="search-bar">
      <InputText v-model="searchQuery" placeholder="Search todos..." />
    </div>


    <div class="add-todo">
      <InputText v-model="newTodoTitle" placeholder="Enter todo title" />
      <Calendar v-model="newTodoDate" dateFormat="yy-mm-dd" placeholder="Select date" />
      <Calendar v-model="newTodoTime" timeOnly hourFormat="24" placeholder="Select time" />
      <Button label="Add" icon="pi pi-plus" severity="success" @click="addTodo" />
    </div>

    <div v-if="hasCompletedTodos" class="toggle-completed">
      <Button :label="hideCompleted ? 'Show Completed' : 'Hide Completed'" @click="toggleHideCompleted" />
    </div>

    <ul class="todo-list">
      <li v-for="todo in visibleTodos" :key="todo.id" :class="{ overdue: isOverdue(todo), completed: todo.isCompleted }"
        class="todo-item">
        <div class="todo-circle" @click.stop="toggleComplete(todo)">
          <div v-if="todo.isCompleted" class="todo-checkmark"></div>
        </div>

        <div class="todo-text" @click="editTodo(todo)">

          <template v-if="editingTodo && editingTodo.id === todo.id">
            <div class="edit-form">
              <InputText v-model="editingTodo.title" placeholder="Edit title" class="edit-input" />

              <div class="edit-date-time">
                <Calendar v-model="editingTodoDate" dateFormat="yy-mm-dd" placeholder="Select date" />
                <Calendar v-model="editingTodoTime" timeOnly hourFormat="24" placeholder="Select time" />
              </div>

              <div class="edit-actions">
                <Button label="Save" icon="pi pi-check" class="mr-2" severity="success" @click.stop="updateTodo" />
                <Button label="Cancel" icon="pi pi-times" severity="secondary" @click.stop="cancelEdit" />
              </div>
            </div>
          </template>

          <template v-else>
            <strong>{{ todo.title }}</strong>
            <div v-if="todo.dueDate" class="due-date">
              Due: {{ formatDueDate(todo.dueDate) }}
            </div>
          </template>
        </div>

        <div class="todo-actions" v-if="!editingTodo || editingTodo.id !== todo.id">
          <Button icon="pi pi-trash" severity="danger" outlined @click.stop="deleteTodo(todo.id)" />
        </div>
      </li>
    </ul>
  </div>
</template>

<script setup>
import { ref, computed, onMounted, watch } from 'vue'
import { createTodoProvider } from '@/services/TodoProviderFactory'

import debounce from 'lodash/debounce'

const searchQuery = ref('')
const debouncedQuery = ref('')

watch(searchQuery, debounce((val) => {
  debouncedQuery.value = val.toLowerCase()
}, 300)) // 300ms delay


const selectedProvider = localStorage.getItem('provider') || 'local'
const provider = createTodoProvider(selectedProvider)

const todos = ref([])
const newTodoTitle = ref('')
const newTodoDate = ref('')
const newTodoTime = ref('')
const hideCompleted = ref(false)
const editingTodo = ref(null)
const editingTodoDate = ref('')
const editingTodoTime = ref('')

const fetchTodos = async () => {
  todos.value = await provider.getTodos()
}

const addTodo = async () => {
  if (!newTodoTitle.value.trim()) return;

  let dueDateTime = null;



  if (newTodoDate.value) {
    const date = new Date(newTodoDate.value);

    // If time is selected, use it; else default to 00:00
    const hours = newTodoTime.value ? newTodoTime.value.getHours() : 0;
    const minutes = newTodoTime.value ? newTodoTime.value.getMinutes() : 0;

    date.setHours(hours);
    date.setMinutes(minutes);

    // Convert to UTC so backend gets correct value
    dueDateTime = new Date(date.getTime() - (date.getTimezoneOffset() * 60000));
  }


  const todo = {
    title: newTodoTitle.value,
    dueDate: dueDateTime,
  };

  await provider.addTodo(todo);
  resetForm();
  fetchTodos();
};


const toggleComplete = async (todo) => {
  todo.isCompleted = !todo.isCompleted
  await provider.updateTodo(todo)
  fetchTodos()
}

const deleteTodo = async (id) => {
  await provider.deleteTodo(id)
  fetchTodos()
}

const editTodo = (todo) => {
  if (editingTodo.value && editingTodo.value.id === todo.id) return;

  editingTodo.value = { ...todo };

  if (todo.dueDate) {
    const date = new Date(todo.dueDate);
    editingTodoDate.value = date;
    editingTodoTime.value = date;
  } else {
    editingTodoDate.value = null;
    editingTodoTime.value = null;
  }
};




const updateTodo = async () => {
  if (!editingTodo.value) return;

  let dueDateTime = null;

  if (editingTodoDate.value) {
    const date = new Date(editingTodoDate.value);
    const hours = editingTodoTime.value ? editingTodoTime.value.getHours() : 0;
    const minutes = editingTodoTime.value ? editingTodoTime.value.getMinutes() : 0;
    date.setHours(hours);
    date.setMinutes(minutes);
    dueDateTime = new Date(date.getTime() - (date.getTimezoneOffset() * 60000));
  }

  editingTodo.value.dueDate = dueDateTime;

  await provider.updateTodo(editingTodo.value);
  editingTodo.value = null;
  editingTodoDate.value = '';
  editingTodoTime.value = '';
  fetchTodos();
};



const cancelEdit = () => editingTodo.value = null

const formatDueDate = (dueDate) => {
  const date = new Date(dueDate)
  return date.toLocaleDateString() + ' ' + date.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })
}

const isOverdue = (todo) => {
  if (!todo.dueDate || todo.isCompleted) return false
  return new Date(todo.dueDate) < new Date()
}

const hasCompletedTodos = computed(() => todos.value.some(todo => todo.isCompleted))

const toggleHideCompleted = () => hideCompleted.value = !hideCompleted.value


const visibleTodos = computed(() => {
  let filtered = todos.value

  if (hideCompleted.value) {
    filtered = filtered.filter(todo => !todo.isCompleted)
  }

  if (debouncedQuery.value) {
    filtered = filtered.filter(todo =>
      todo.title.toLowerCase().includes(debouncedQuery.value)
    )
  }

  return filtered.sort((a, b) => {
    if (!a.dueDate && !b.dueDate) return 0
    if (!a.dueDate) return -1
    if (!b.dueDate) return 1
    return new Date(a.dueDate) - new Date(b.dueDate)
  })
})


const resetForm = () => {
  newTodoTitle.value = ''
  newTodoDate.value = ''
  newTodoTime.value = ''
}

onMounted(() => fetchTodos())
</script>

<style scoped>


.container {
  max-width: 700px;
  margin: 2rem auto;
  padding: 2rem;
  background: rgba(255, 255, 255, 0.85);
  border-radius: 16px;
  box-shadow: 0 8px 24px rgba(0, 0, 0, 0.15);
  backdrop-filter: blur(8px);
  border: 1px solid rgba(255, 255, 255, 0.25);
}


.add-todo {
  display: flex;
  flex-wrap: wrap;
  gap: 0.75rem;
  margin-bottom: 2rem;
  background-color: #f5f7fa;
  padding: 1rem;
  border-radius: 10px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.03);
}

.todo-list {
  list-style: none;
  padding: 0;
}

.todo-item {
  background: #fafafa;
  margin-bottom: 1rem;
  padding: 1rem;
  border-radius: 12px;
  box-shadow: 0 4px 10px rgba(0, 0, 0, 0.05);
  display: flex;
  align-items: center;
  transition: 0.3s;
}

.todo-item:hover {
  background: #f0f8ff;
  transform: translateY(-3px);
  box-shadow: 0 6px 14px rgba(0, 0, 0, 0.1);
}



.todo-circle {
  width: 24px;
  height: 24px;
  border: 2px solid #ff6a7f; 
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  cursor: pointer;
}

.todo-checkmark {
  background: linear-gradient(to right, #ff6a7f, #845ec2); /* pink to violet */
  width: 16px;
  height: 16px;
  border-radius: 50%;
  font-size: 12px;
  display: flex;
  align-items: center;
  justify-content: center;
}

.todo-text {
  flex: 1;
  padding-left: 1rem;
  cursor: pointer;
}

.due-date {
  font-size: 0.8rem;
  color: gray;
}

.completed .todo-text strong {
  text-decoration: line-through;
  color: gray;
}

.overdue {
  background: #ffeaea;
}

.edit-form {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.edit-date-time {
  display: flex;
  gap: 0.75rem;
  flex-wrap: wrap;
}

.edit-actions {
  display: flex;
  gap: 0.75rem;
  flex-wrap: wrap;
}

.search-bar {
  margin-bottom: 1rem;
}
.search-bar input {
  width: 100%;
  padding: 0.6rem;
  font-size: 1rem;
  border-radius: 6px;
  border: 1px solid #ccc;
}

.p-button {
  border-radius: 50px !important;
  font-weight: 600;
}

.p-button-success,
.p-button-secondary {
  background: linear-gradient(90deg, #FF5F6D, #845EC2) !important;
  border: none !important;
  color: white !important;
  font-weight: 600;
}


.p-button-success:hover,
.p-button-secondary:hover {
  filter: brightness(1.1);
}

.p-button-danger.p-button-outlined {
  background: transparent !important;
  border: 2px solid transparent !important;
  color: #FF5F6D !important;
  border-image: linear-gradient(90deg, #FF5F6D, #845EC2) 1 !important;
}

.p-button-danger.p-button-outlined:hover {
  background: linear-gradient(90deg, #FF5F6D, #845EC2) !important;
  color: white !important;
  border: none !important;
}



.toggle-completed .p-button {
  background: linear-gradient(to right, #ff6a7f, #845ec2);
  color: white;
  border: none;
  border-radius: 999px;
  font-weight: 600;
  padding: 0.5rem 1.25rem;
  transition: box-shadow 0.3s ease;
}

.toggle-completed .p-button:hover {
  box-shadow: 0 4px 12px rgba(132, 94, 194, 0.4);
}


:global(body) {
  margin: 0;
  font-family: 'Inter', sans-serif;
  background: linear-gradient(135deg, #FF5F6D, #845EC2);
  background-attachment: fixed;
  background-size: cover;
}

</style>
