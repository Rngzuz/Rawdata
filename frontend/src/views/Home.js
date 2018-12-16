import SearchService from 'Services/SearchService.js'
import { observableArray } from 'knockout'
import { Component, wrapComponent } from 'Components/Component.js'

class Home extends Component {
    constructor(args) {
        super(args)

        this.words = this.$store.getters.searchParams
        this.items = observableArray()

        this.isLoading(true)
        this.fetchItems()

        this.$store.subscribe('searchParams', value => {
            this.isLoading(true)
            this.fetchItems(value)
        })
    }

    async fetchItems(words) {
        let result

        if (!words || words.length <= 0) {
            result = await SearchService.getNewest()

            result = result.map(item => {
                return {
                    ...item,
                    body: item.body
                        .substring(0, 500)
                        .replace(/<(?:.|\n)*?>/gm, '')
                        .trimStart()
                        .trimEnd()
                }
            })

        } else {
            result = await SearchService.getBestMatch(words, 1, 50)

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
            this.isLoading(false)
            this.items(result)
        }, 1100)
    }
}

const template = /* html */ `
<div data-bind="visible: !isLoading()" class="card">
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
                <div data-bind="highlightText: $data.body, units: $component.words()">
                    <span>...</span>
                </div>
                <cite class="d-block mt-3" data-bind="attr: { title: $data.authorDisplayName }">
                    <span class="text-muted" data-bind="text: ' - ' + $data.authorDisplayName"></span>
                </cite>
            </article>
        </li>
    </ul>
</div>
`

export default wrapComponent(Home, template)