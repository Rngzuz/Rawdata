import Store from './Store.js'
import AuthService from 'Services/AuthService.js'
import Router from './Router.js'

class App {
    constructor() {
        if (AuthService.hasToken()) {
            Store.commit('SET_IS_AUTHENTICATED', true)
        }

        this.isLoading = Store.getters.isLoading
    }

    getComponent() {
        const route = Router.currentRoute()

        return {
            name: route.component,
            params: route.params
        }
    }
}

const template = /* html */ `
<so-navbar id="navbar"></so-navbar>

<main class="container py-3 py-lg-5" data-bind="component: getComponent()"></main>

<!-- ko if: isLoading -->
    <so-loader id="loader" params="size: 200"></so-loader>
<!-- /ko -->
`

export default { name: 'so-app', viewModel: App, template }