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

// const initStore = {

//     state: {
//         search: observableArray([]),
//         isLoading: observable(true)
//     },

//     getters: {
//         search: state => state.search(),
//         isLoading: state => state.isLoading(),
//         test: 'string'
//     },

//     mutations: {
//         SET_TEST(state, payload) {
//             state.test = 'hello' + payload
//         },
//         SET_SEARCH(state, payload) {
//             state.search(payload)
//         }
//     },

//     actions: {
//         async doSomethingWithSearch(context) {
//             const { state, commit } = context

//             const data = await getSomeRandomData()

//             commit('SET_SEARCH', data)
//         }
//     }

// }

export default new Store({
    search: observableArray([]),
    isLoading: observable(true)
})
