import Store from '@/Store.js'
import SearchService from 'Services/SearchService.js'
import { observableArray, computed } from 'knockout'

class SignIn {
    constructor() {
        this.isLoading = computed({
            read: Store.getters.isLoading,
            write: newValue => Store.dispatch('updateIsLoading', newValue)
        })
    }
}

const template = /* html */ `
<h1>SignIn</h1>
`

export default { viewModel: SignIn, template }