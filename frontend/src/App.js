import Store from './Store.js'
import { pureComputed } from 'knockout'

class App {
    constructor() {
        this.isLoading = pureComputed(() => Store.state().isLoading())
    }
}

const template = /* html */`
<so-navbar id="navbar"></so-navbar>
<so-loader id="loader" data-bind="visible: isLoading" params="size: 200"></so-loader>

<main id="content" class="container">
    <so-home></so-home>
</main>
`

export default { name: 'so-app', viewModel: App, template }