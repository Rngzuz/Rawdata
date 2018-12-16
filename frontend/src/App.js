import Store from './Store.js'
import Router from './Router.js'

class App {
    constructor() {
        this.isLoading = Store.getters.isLoading
        this.route = Router.currentRoute
    }
}

const template = /* html */ `
<so-navbar id="navbar"></so-navbar>

<main id="content" class="container" data-bind="component: route().component" params="routeParams: route().params"></main>

<so-loader id="loader" data-bind="visible: isLoading()" params="size: 200"></so-loader>
<!--<so-search></so-search>-->
`

export default { name: 'so-app', viewModel: App, template }