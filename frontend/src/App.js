import Store from './Store.js'
import Router from './Router.js'

class App {
    constructor() {
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

<main id="content" class="container" data-bind="component: getComponent()"></main>

<so-loader id="loader" data-bind="visible: isLoading()" params="size: 200"></so-loader>
<!--<so-search></so-search>-->
`

export default { name: 'so-app', viewModel: App, template }