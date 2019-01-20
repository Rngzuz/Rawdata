import Store from '@/Store.js'
import Router from '@/Router.js'
import { observable, observableArray } from 'knockout'

class Post {
    constructor(params = {}) {
        const {
            post = {},
            toggle = () => { },
            showLink = false,
            showComments = false
        } = params

        this.showPrompt = observable(false)
        this.selectedComment = {}

        this.postWasToggled = false

        if (showComments) {
            this.comments = observableArray(post.comments)
        }

        this.post = post

        this.toggle = toggle
        this.showLink = showLink
        this.showComments = showComments

        this.isAuthenticated = Store.getters.isAuthenticated
    }

    formatDate(date) {
        return new Date(date).toLocaleDateString('en-GB', { year: 'numeric', month: 'long', day: 'numeric'})
    }

    showPromptOnTogglePost(post) {
        if (post.marked) {
            this.toggle({ ...post, note: '' })
        } else {
            this.postWasToggled = true
            this.showPrompt(true)
        }
    }

    showPromptOnToggleComment(comment) {
        if (comment.marked) {
            this.toggleComment({ ...comment, note: '' })
        } else {
            this.selectedComment = comment
            this.showPrompt(true)
        }
    }

    toggleComment(comment) {
        const oldComment = this.comments.peek()
            .find(item => item.id === comment.id)

        const newComment = { ...oldComment, marked: !oldComment.marked, note: comment.note }
        this.comments.replace(oldComment, newComment)

        Store.dispatch('toggleMarkComment', { id: comment.id, note: comment.note })
    }

    navigateToQuestion() {
        Router.setRoute('question', { id: this.post.questionId })
    }
}

const template = /* html */ `
<article class="card card-post">
    <div class="card-body">
        <!-- ko if: post.title -->
        <h5 class="card-title text-truncate mb-3" data-bind="html: post.title"></h5>
        <!-- /ko -->
        <aside class="float-left pr-3">
            <!-- ko if: $component.isAuthenticated() -->
            <div class="card-star" data-bind="click: ({ post }, event) => { showPromptOnTogglePost(post) }">
                <i class="fa-star" data-bind="css: { fas: post.marked, far: !post.marked }"></i>
                <div class="card-star-title">mark</div>
            </div>
            <!-- /ko -->
            <div class="card-score">
                <div class="card-score-count" data-bind="html: post.score"></div>
                <div class="card-score-title">score</div>
            </div>
        </aside>
        <div data-bind="html: post.body"></div>
        <!-- ko if: post.note -->
        <div class="mt-2 text-info font-weight-bold" data-bind="text: 'Note: ' + post.note"></div>
        <!-- /ko -->
    </div>
    <footer class="card-footer bg-white d-flex justify-content-between align-items-center">
        <div class="text-muted">
            <span>by</span>
            <cite data-bind="text: post.authorDisplayName"></cite>
            <span>on the <time data-bind="text: $component.formatDate(post.creationDate)"></time></span>
        </div>
        <!-- ko if: showLink -->
        <button type="button" class="btn btn-primary btn-sm" data-bind="click: navigateToQuestion">Read more</button>
        <!-- /ko -->
    </footer>
</article>

<!-- ko if: showComments -->
<ul class="list-group mx-3 mx-lg-auto" data-bind="foreach: comments">
    <li class="list-group-item">
        <aside class="float-left pr-3">
            <!-- ko if: $component.isAuthenticated() -->
            <div class="card-star" data-bind="click: (comment, event) => $component.showPromptOnToggleComment(comment)">
                <i class="fas fa-star" data-bind="css: { fas: $data.marked, far: !$data.marked }"></i>
                <div class="card-star-title">mark</div>
            </div>
            <!-- /ko -->
            <div class="card-score">
                <div class="card-score-count" data-bind="text: $data.score"></div>
                <div class="card-score-title">score</div>
            </div>
        </aside>

        <p class="mb-1 clearfix" data-bind="text: $data.text"></p>
        <p class="" data-bind="text: $data.note"></p>
        <footer class="text-muted text-right small">
            <span>by</span>
            <cite data-bind="text: $data.authorDisplayName"></cite>
            <span>on the <time data-bind="text: $component.formatDate($data.creationDate)"></time></span>
        </footer>
    </li>
</ul>
<!-- /ko -->

<!-- ko component: {
    name: 'so-prompt',
    params: {
        showPrompt,
        onAccept: value => {
            showPrompt(false)

            if (postWasToggled) {
                toggle({ ...post, note: value })
                postWasToggled = false
            } else if (showComments) {
                toggleComment({ ...selectedComment, note: value, marked: !selectedComment.marked })
            }
        },
        onCancel: () => {
            showPrompt(false)
        }
    }
} --><!-- /ko -->
`

export default { viewModel: Post, template }