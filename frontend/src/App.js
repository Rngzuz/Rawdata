import Store from './Store.js'
import { pureComputed } from 'knockout'

class App {
    constructor() {
        this.isLoading = pureComputed(Store.getters.isLoading)
    }
}

const template = /* html */ `
<so-navbar id="navbar"></so-navbar>

<main id="content" class="container">
    <so-home></so-home>
    <!--<so-word-cloud params="word: 'mac'"></so-word-cloud>-->
</main>

<so-loader id="loader" data-bind="visible: isLoading()" params="size: 200"></so-loader>
<!--<so-search></so-search>-->
`

export default { name: 'so-app', viewModel: App, template }