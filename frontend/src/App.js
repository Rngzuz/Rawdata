import Store from './Store.js'
import { pureComputed } from 'knockout'
import { BaseService } from 'Services/BaseService.js'


class App {
    constructor() {
        this.isLoading = pureComputed(
            () => Store.state().isLoading()
        )
    }
}

const template = /* html */ `
<so-navbar id="navbar"></so-navbar>

<header id="banner" class="bg-info border-bottom">
    <div style="height: 400px;"></div>
</header>

<main id="content" class="container">
    <so-home></so-home>
</main>

<so-loader id="loader" data-bind="visible: isLoading" params="size: 200"></so-loader>
`

export default { name: 'so-app', viewModel: App, template }