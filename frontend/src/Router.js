import { observable } from 'knockout'

class Router {
    constructor(routes) {
        this.routes = routes
        this.currentRoute = observable({ ...routes[0], params: {} })
    }

    setRoute(routeName, params = {}) {
        const route = this.routes
            .find(route => route.name === routeName)

        if (routeName !== this.currentRoute().name) {
            this.currentRoute({ ...route, params })
        }
    }

    getRoute() {
        return this.currentRoute()
    }
}

export default new Router([
    {
        name: 'home',
        component: 'so-home',
        title: 'Home'
    },
    {
        name: 'sign-in',
        component: 'so-sign-in',
        title: 'Sign in'
    },
    {
        name: 'user-profile',
        component: 'so-user-profile',
        title: 'User profile'
    },
    {
        name: 'question',
        component: 'so-question',
        title: 'Question'
    },
    {
        name: 'failure',
        component: 'so-failure',
        title: 'Failure'
    },
    {
        name: 'force-graph',
        component: 'so-force-graph',
        title: 'Force Graph'
    }
])