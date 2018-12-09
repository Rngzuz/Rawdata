import { observable } from 'knockout'

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
    testData: 'Hello World!'
})
