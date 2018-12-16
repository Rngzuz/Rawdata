import SearchService from 'Services/SearchService.js'
import { observableArray } from 'knockout'
import { Component, wrapComponent } from 'Components/Component.js'
import { getPlainExcerpt, getMarkedExcerpt, stripHtmlAndMark } from 'Bindings/highlightText.js'

class Home extends Component {
    constructor(args) {
        super(args)

        this.words = this.$store.getters.searchParams
        this.items = observableArray()

        this.fetchPosts(
            this.$store.getters.searchParams()
        )

        this.$store.subscribe('searchParams', words => {
            this.fetchPosts(words)
        })
    }

    async fetchPosts(words) {
        this.isLoading(true)

        let posts

        if (words.length > 0) {
            const json = await SearchService
                .getBestMatch(words, 1, 50)

            posts = json.items.map(post => {
                const newPost = {
                    ...post,
                    body: getMarkedExcerpt(post.body, words)
                }

                if (post.title) {
                    newPost.title = stripHtmlAndMark(post.title, words)
                }

                return newPost
            })
        } else {
            const json = await SearchService
                .getNewest()

            posts = json.map(post => {
                return {
                    ...post,
                    body: getPlainExcerpt(post.body)
                }
            })
        }

        this.items(posts)
        this.isLoading(false)
    }

    navigate(event, routeName, params = {}) {
        event.preventDefault()
        this.$router.setRoute(routeName, params)
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
                <h5 class="card-title" data-bind="visible: $data.title, html: $data.title"></h5>
                <div data-bind="html: $data.body + '...'"></div>
                <cite class="d-block mt-3" data-bind="attr: { title: $data.authorDisplayName }">
                    <span class="text-muted" data-bind="text: ' - ' + $data.authorDisplayName"></span>
                </cite>
                <button type="button" data-bind="click: (_, event) => $component.navigate(event, 'question', { id: $data.questionId })">Read more</button>
            </article>
        </li>
    </ul>
</div>
`

export default wrapComponent(Home, template)