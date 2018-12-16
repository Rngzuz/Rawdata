import Store from './Store.js'
import { observable } from 'knockout'

class App {
    constructor() {
        this.isLoading = Store.getters.isLoading
        this.currentView = observable('so-home')
    }
}

const template = /* html */ `
<so-navbar id="navbar" params="currentView: currentView"></so-navbar>

<main id="content" class="container" data-bind="component: currentView()"></main>

<so-loader id="loader" data-bind="visible: isLoading()" params="size: 200"></so-loader>
<!--<so-search></so-search>-->
`

export default { name: 'so-app', viewModel: App, template }