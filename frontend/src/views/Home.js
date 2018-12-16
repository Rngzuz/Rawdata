import SearchService from 'Services/SearchService.js'
import { observableArray } from 'knockout'
import { Component, wrapComponent } from 'Components/Component.js'
import { getPlainExcerpt, getMarkedExcerpt, escapeHtmlAndMark, escapeHtml } from 'Bindings/highlightText.js'

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
                    newPost.title = escapeHtmlAndMark(post.title, words)
                }

                return newPost
            })
        } else {
            const json = await SearchService
                .getNewest()

            posts = json.map(post => {

                const newPost = {
                    ...post,
                    body: getPlainExcerpt(post.body)
                }

                if (post.title) {
                    newPost.title = escapeHtml(post.title, words)
                }

                return newPost
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


<section data-bind="visible: !isLoading(), foreach: items">
    <article class="card mb-3">
        <section class="card-body">
            <h5 class="card-title" data-bind="visible: $data.title, html: $data.title"></h5>
            <div class="mb-4" data-bind="html: $data.body + '...'"></div>
            <button type="button" class="btn btn-primary btn-sm"
                    data-bind="click: (_, event) => $component.navigate(event, 'question', { id: $data.questionId })">Read more</button>
        </section>
        <footer class="card-footer text-muted">
            <cite data-bind="text: $data.authorDisplayName"></cite>
        </footer>
    </article>
</section>

`

export default wrapComponent(Home, template)