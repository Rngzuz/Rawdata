import Store from '@/Store.js'
import SearchService from 'Services/SearchService.js'
import { observableArray, pureComputed } from 'knockout'

class Home {
    constructor() {
        this.isLoading = pureComputed(
            () => Store.state().isLoading()
        )

        this.words = pureComputed(
            () => Store.state().search()
        )

        this.items = observableArray()
        this.fetchItems(this.words())

        Store.state().search.subscribe(v => {
            Store.state().isLoading(true)
            this.fetchItems(this.words())
        })
    }

    async fetchItems(words) {
        let result

        if (words.length <= 0) {
            result = await SearchService.getNewest()
        } else {
            result = await SearchService.getBestMatch(words, 1, 50)

            console.log(result)

            result = result.items.map(item => {
                const body = item.excerpts
                    .map(sentence => {
                        // Need to copy the string by value to use the g flag in regex
                        const tmpSentence = sentence

                        return tmpSentence
                            .replace(/\s*([^\w\s\.\,]|n't)\s*/gi, '$1')
                            .replace(/[\s\.]*$/s, '')
                    })
                    .join(' ... ')
                return { ...item, body }
            })
        }

        setTimeout(() => {
            Store.state().isLoading(false)
            this.items(result)
        }, 1100)
    }
}

const template = /* html */ `
<div data-bind="visible: !isLoading() && items().length > 0" class="card">
    <ul class="list-group list-group-flush" data-bind="foreach: items">
        <li class="list-group-item d-flex justify-content-between align-items-center py-4">
            <div class="mr-3 text-center flex-shrink-1">
                <span class="d-block badge badge-primary badge-pill" data-bind="text: score"></span>
                <small class="d-block text-muted">score</small>
            </div>
            <article class="flex-grow-1">
                <h5 class="card-title" data-bind="visible: $data.title !== undefined">
                    <span data-bind="highlightText: $data.title, units: $component.words()"></span>
                </h5>
                <p data-bind="highlightText: $data.body, units: $component.words()"></p>
                <cite class="d-block mt-3" data-bind="attr: { title: $data.authorDisplayName }">
                    <span class="text-muted" data-bind="text: ' - ' + $data.authorDisplayName"></span>
                </cite>
            </article>
        </li>
    </ul>
</div>
`

export default { viewModel: Home, template }