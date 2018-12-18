import { observable, observableArray } from 'knockout'
import { reduce } from 'lodash-es'

// context: state, getters, commit, payload
const createCommit = context => (type, payload) => {
    return context
        .mutations[type]
        .call(context, context.state, payload)
}

// context: state, getters, commit, payload
const createDispatch = (context, commit) => (type, payload) => {
    return context
        .actions[type]
        .call(context, {
            state: context.state,
            getters: context.getters,
            commit,
            payload
        })
}

const createSubscribe = context => (index, callback) => {
    return context.state[index]
        .subscribe(callback)
}

// init: state, getters, mutations, actions
const createStore = context => {
    const reduceGetters = (result, getter, key) => {
        result[key] = getter(context.state)
        return result
    }

    context.getters = reduce(context.getters, reduceGetters, {})

    const subscribe = createSubscribe(context)
    const commit = createCommit(context)
    const dispatch = createDispatch(context, commit)

    return { getters: context.getters, subscribe, commit, dispatch }
}

export default createStore({
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
            console.log(this)
            state.isLoading(payload)
        },
        SET_SEARCH_PARAMS(state, payload) {
            state.searchParams(payload)
        }
    },
    actions: {
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