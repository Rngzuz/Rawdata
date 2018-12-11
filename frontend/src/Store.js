import { omit } from 'lodash-es'
import { observable, observableArray } from 'knockout'

class Store {
    constructor(initialState = {}) {
        this.state = observable(initialState)
    }

    set(payload) {
        this.state(payload)
    }

    update(payload) {
        const newState = { ...this.state, ...payload }
        this.state(newState)
    }

    delete(payload) {
        const newState = omit({ ...this.state() }, payload)
        this.state(newState)
    }
}

export default new Store({
    search: observableArray([]),
    isLoading: observable(true)
})
