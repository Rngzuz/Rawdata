import UserService from 'Services/UserService.js'
import { observable, observableArray } from 'knockout'
import { reduce } from 'lodash-es'

class Store {
    constructor({ state, getters, mutations, actions }) {
        const invokeGetters = (accumulator, getter, key) => {
            accumulator[key] = getter(state)
            return accumulator
        }

        Object.defineProperties(this, {
            state: { value: state },
            getters: { value: reduce(getters, invokeGetters, {}) },
            mutations: { value: mutations },
            actions: { value: actions }
        })

        this.subscribe = this.subscribe.bind(this)
        this.commit = this.commit.bind(this)
        this.dispatch = this.dispatch.bind(this)
    }

    subscribe(index, callback) {
        return this.state[index]
            .subscribe(callback)
    }

    commit(type, payload) {
        return this.mutations[type]
            .call(this, this.state, payload)
    }

    dispatch(type, payload) {
        return this.actions[type]
            .call(this, {
                state: this.state,
                getters: this.getters,
                commit: this.commit,
                payload
            })
    }
}

export default new Store({
    state: {
        isAuthenticated: observable(false),
        isLoading: observable(false),
        searchParams: observableArray([])
    },
    getters: {
        isAuthenticated: state => state.isAuthenticated,
        isLoading: state => state.isLoading,
        searchParams: state => state.searchParams
    },
    mutations: {
        SET_IS_AUTHENTICATED(state, payload) {
            state.isAuthenticated(payload)
        },
        SET_IS_LOADING(state, payload) {
            state.isLoading(payload)
        },
        SET_SEARCH_PARAMS(state, payload) {
            state.searchParams(payload)
        }
    },
    actions: {
        async toggleMarkPost({ payload }) {
            await UserService.toggleMarkedPost(payload.id, payload.note)
        },
        async toggleMarkComment({ payload }) {
            await UserService.toggleMarkedComment(payload.id, payload.note)
        },
        updateIsAuthenticated({ commit, payload }) {
            commit('SET_IS_AUTHENTICATED', payload)
        },
        updateIsLoading({ commit, payload }) {
            commit('SET_IS_LOADING', payload)
        },
        updateSearchParams({ commit, payload }) {
            const searchParams = /\w/.test(payload) ?
                payload.split(/\s/) : []

            commit('SET_SEARCH_PARAMS', searchParams)
        }
    }
})