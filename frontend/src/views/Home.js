import SearchService from 'Services/SearchService.js'
import { observable, observableArray } from 'knockout'
import { Component, wrapComponent } from 'Components/Component.js'
import { getPlainExcerpt, getMarkedExcerpt, escapeHtmlAndMark, escapeHtml } from 'Bindings/highlightText.js'

class Home extends Component {
    constructor(args) {
        super(args)

        this.words = this.$store.getters.searchParams
        this.items = observableArray([])

        this.currentPage = observable(1)
        this.pageCount = observable(1)

        this.fetchPosts(
            this.$store.getters.searchParams()
        )

        this.$store.subscribe('searchParams', words => {
            this.fetchPosts(words, this.currentPage.peek())
        })

        this.currentPage.subscribe(currentPage => {
            this.fetchPosts(this.words.peek(), currentPage)
        })
    }

    previousPage() {
        const currentPage = this.currentPage.peek()

        if (currentPage > 1) {
            this.currentPage(currentPage - 1)
        }
    }

    nextPage() {
        const currentPage = this.currentPage.peek()

        if (currentPage + 1 !== this.currentPage.peek()) {
            this.currentPage(currentPage + 1)
        }
    }

    async fetchPosts(words, page = 1, size = 50) {
        this.isLoading(true)

        let posts

        if (words.length > 0) {
            const json = await SearchService
                .getBestMatch(words, page, size)

            this.currentPage(json.currentPage)
            this.pageCount(json.pageCount)

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

            this.pageCount(0)

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

        console.log(posts)

        this.items(posts)
        this.isLoading(false)
    }
}

const template = /* html */ `
<!-- ko if: !isLoading() -->
    <section data-bind="component: { name: 'so-list', params: { items } } "></section>

    <!-- ko if: (pageCount() > 1) -->
        <div class="text-center">
            <b data-bind="text: 'page ' + currentPage()"></b>
        </div>
        <nav class="d-flex justify-content-center mt-3">
            <ul class="pagination">
                <li class="page-item" data-bind="css: { disabled: (currentPage() === 1) }">
                    <a class="page-link" data-bind="click: previousPage">Previous</a>
                </li>
                <li class="page-item" data-bind="css: { disabled: (currentPage() === pageCount()) }">
                    <a class="page-link" data-bind="click: nextPage">Next</a>
                </li>
            </ul>
        </nav>
    <!-- /ko -->
<!-- /ko -->
`

export default wrapComponent(Home, template)