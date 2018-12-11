import { observableArray, pureComputed } from 'knockout'
import SearchService from '@/services/SearchService.js'
import Store from '@/Store.js'

class Home {
    constructor() {
        this.words = pureComputed(() => Store.state().search())
        this.fetchItems(this.words())
        this.items = observableArray()
        this.isLoading = pureComputed(() => Store.state().isLoading())


        Store.state().search.subscribe(v => {
            Store.state().isLoading(true)
            this.fetchItems(this.words())
        })
    }

    async fetchItems(words) {
        const result = await SearchService.getBestMatch(words, 1, 50)

        const items = result.map(item => {
            const newBody = item.body.replace(/<(?:.|\n)*?>/gm, '')
            return { ...item, body: newBody }
        })

        console.log(items)

        setTimeout(() => {
            Store.state().isLoading(false)
            this.items(items)
        }, 2000)
    }
}

const template = /* html */ `
<div data-bind="visible: !isLoading() && items().length > 0" class="card">
    <ul class="list-group list-group-flush" data-bind="foreach: items">
        <li class="list-group-item d-flex justify-content-between align-items-center py-4">
            <div class="mr-3 text-center">
                <span class="badge badge-primary badge-pill" data-bind="text: score"></span>
                <small class="text-muted">score</small>
            </div>
            <article>
                <h5 class="card-title" data-bind="visible: $data.title !== undefined">
                    <span data-bind="highlightText: $data.title, units: $component.words()"></span>
                </h5>
                <div data-bind="highlightText: body.substring(0, 350), units: $component.words()"></div>
                <cite class="d-block mt-3" data-bind="attr: { title: $data.authorDisplayName }">
                    <span class="text-muted" data-bind="text: ' - ' + $data.authorDisplayName"></span>
                </cite>
            </article>
        </li>
    </ul>
</div>
`

export default { viewModel: Home, template }