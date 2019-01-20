import Store from '@/Store.js'
import SearchService from 'Services/SearchService.js'
import { observable, observableArray } from 'knockout'
import { getPlainExcerpt, getMarkedExcerpt, escapeHtmlAndMark, escapeHtml } from 'Helpers/highlightText.js'

class Home {
    constructor() {
        this.isLoading = Store.getters.isLoading
        this.words = Store.getters.searchParams
        this.items = observableArray([])

        this.currentPage = observable(1)
        this.pageCount = observable(1)

        this.searchAlgo = observable('best')

        this.fetchPosts(
            Store.getters.searchParams()
        )

        const searchParamsSub = Store.subscribe('searchParams', words => {
            this.fetchPosts(words, this.currentPage.peek())
        })

        const currentPageSub = this.currentPage.subscribe(currentPage => {
            this.fetchPosts(this.words.peek(), currentPage)
        })

        this.dispose = function () {
            searchParamsSub.dispose()
            currentPageSub.dispose()
        }

        this.togglePost = this.togglePost.bind(this)
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
            let json

            switch (this.searchAlgo()) {
                case 'best':
                    json = await SearchService
                        .getBestMatch(words, page, size)
                    break
                case 'exact':
                    json = await SearchService
                        .getExactMatch(words, page, size)
                    break
                case 'ranked':
                    json = await SearchService
                        .getRankedWeightedMatch(words, page, size)
                    break
            }

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

        this.items(posts)
        this.isLoading(false)
    }

    togglePost(post) {
        const oldPost = this.items.peek()
            .find(item => item.id === post.id)

        const newPost = { ...post, marked: !oldPost.marked }
        this.items.replace(oldPost, newPost)

        Store.dispatch('toggleMarkPost', { id: post.id, note: newPost.note })
    }

}

const template = /* html */ `
<!-- ko if: !isLoading() -->
    <div class="form-inline mb-3">
        <div class="form-check">
            <label class="form-check-label">
                <input class="form-check-input" type="radio" value="best" data-bind="checked: searchAlgo" checked>
                <span>Best match</span>
            </label>
        </div>
        <div class="form-check mx-4">
            <label class="form-check-label">
                <input class="form-check-input" type="radio" value="exact" data-bind="checked: searchAlgo">
                <span>Exact match</span>
            </label>
        </div>
        <div class="form-check">
            <label class="form-check-label">
                <input class="form-check-input" type="radio" value="ranked" data-bind="checked: searchAlgo">
                <span>Ranked weighted match</span>
            </label>
        </div>
    </div>

    <!-- ko foreach: items -->
        <div class="mt-4" data-bind="component: { name: 'so-post', params: { post: $data, toggle: $component.togglePost, showLink: true } }"></div>
    <!-- /ko -->

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

export default { viewModel: Home, template }