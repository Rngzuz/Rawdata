import { observable, observableArray } from 'knockout'
import { reduce } from 'lodash-es'

class Store {
    constructor(initStore) {
        const { state, getters = {}, mutations, actions } = initStore

        const commit = (type, payload) =>
            mutations[type]
            .call(this, state, payload)

        const dispatch = (type, payload) =>
            actions[type]
            .call(this, {
                state,
                getters: this.getters,
                commit,
                payload
            })

        const reduceGetters = (accumulator, getter, key) => {
            accumulator[key] = getter(state)
            return accumulator
        }

        const subscribe = (key, fn) => state[key].subscribe(fn)

        this.commit = commit
        this.dispatch = dispatch
        this.getters = reduce(getters, reduceGetters, {})
        this.subscribe = subscribe
    }
}

export default new Store({
    state: {
        isLoading: observable(true),
        searchParams: observableArray([])
    },
    getters: {
        isLoading: state => state.isLoading,
        searchParams: state => state.searchParams
    },
    mutations: {
        SET_IS_LOADING(state, payload) {
            state.isLoading(payload)
        },
        SET_SEARCH_PARAMS(state, payload) {
            state.searchParams(payload)
        }
    },
    actions: {
        updateIsLoading({ state, commit, payload }) {
            commit('SET_IS_LOADING', payload)
        },
        updateSearchParams({ commit, payload }) {
            const searchParams = /\w/.test(payload) ?
                payload.split(/\s/) : []

            commit('SET_SEARCH_PARAMS', searchParams)
        }
    }
})