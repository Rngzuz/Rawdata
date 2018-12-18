import Store from '@/Store.js'
import Router from '@/Router.js'

class List {
    constructor(params = {}) {
        this.searchParams = Store.getters.searchParams
        this.isAuthenticated = Store.getters.isAuthenticated
        this.items = params.items
    }

    navigate(event, routeName, params = {}) {
        event.preventDefault()
        Router.setRoute(routeName, params)
    }

    toggleMarkPost(data) {
        const newItems = this.items.peek().map(post => {
            if (post.id === data.id) {
                return { ...post, marked: !post.marked }
            }

            return post
        })

        this.items(newItems)
        Store.dispatch('toggleMarkPost', data.id)
    }
}

const template = /* html */ `
<!-- ko foreach: items -->
<article class="card mb-3">
    <div class="card-body">
        <!-- ko if: $data.title -->
        <h5 class="card-title mb-2" data-bind="html: $data.title"></h5>
        <!-- /ko -->
        <div class="d-flex">
            <aside class="flex-shrink-1 mr-3">
                <div class="score text-center">
                    <span class="d-block badge badge-primary" data-bind="text: $data.score"></span>
                    <small class="d-block">score</small>
                </div>
            </aside>

            <div class="flex-grow-1">
                <div data-bind="html: $data.body + '...'"></div>
                <div class="mt-2 text-info font-weight-bold" data-bind="visible: $data.note, text: 'Note: ' + $data.note"></div>
            </div>

            <!-- ko if: $component.isAuthenticated() -->
            <i class="flex-shrink-1 align-self-center ml-3 text-warning fa-star fa-2x" data-bind="click: () => $component.toggleMarkPost($data), css: { fas: $data.marked, far: !$data.marked }"></i>
            <!-- /ko -->
        </div>
    </div>
    <footer class="card-footer bg-white d-flex align-items-center justify-content-between">
        <div class="text-muted">
            <span>by</span>
            <cite data-bind="text: $data.authorDisplayName"></cite>
            <span>on the <time data-bind="text: $data.creationDate"></time></span>
        </div>
        <button type="button" class="btn btn-outline-dark btn-sm" data-bind="click: (_, event) => $component.navigate(event, 'question', { id: $data.questionId })">Read
            more</button>
    </footer>
</article>
<!-- /ko -->
`

export default { viewModel: List, template }