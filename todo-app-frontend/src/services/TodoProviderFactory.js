import { ApiProvider } from './ApiProvider'
import { LocalProvider } from './LocalProvider'

export function createTodoProvider() {
  const provider = localStorage.getItem('provider') || 'api'

  if (provider === 'local') {
    return LocalProvider
  } else {
    return ApiProvider
  }
}
