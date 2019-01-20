import Store from '@/Store.js'
import { observable, observableArray } from 'knockout'
import UserService from '../services/UserService.js'
import { escapeHtml, getPlainExcerpt } from 'Helpers/highlightText'

class UserProfile {
    constructor() {
        this.isLoading = Store.getters.isLoading

        this.profile = observable({})
        this.markedPosts = observableArray([])
        this.markedComments = observableArray([])
        this.searchHistory = observableArray([])

        this.fetchUserProfile()
        this.togglePost = this.togglePost.bind(this)
    }

    async fetchUserProfile() {
        this.isLoading(true)

        let result = await UserService.getUserProfile()

        result.markedPosts = result.markedPosts.map(post => {
            const newPost = {
                ...post,
                body: getPlainExcerpt(post.body)
            }

            if (post.title) {
                newPost.title = escapeHtml(post.title)
            }

            return newPost
        })

        this.profile(result)
        this.markedPosts(result.markedPosts)
        this.markedComments(result.markedComments)
        this.searchHistory(result.searchHistory)

        console.dir(this.searchHistory())

        this.isLoading(false)
    }

    togglePost(post) {
        const oldPost = this.markedPosts.peek()
            .find(answer => answer.id === post.id)

        const newPost = { ...oldPost, marked: !oldPost.marked }
        this.markedPosts.replace(oldPost, newPost)

        Store.dispatch('toggleMarkPost', { id: post.id, note: '' })
    }

    toggleComment(comment) {
        const oldComment = this.markedComments.peek()
            .find(item => item.id === comment.id)

        const newComment = { ...oldComment, marked: !oldComment.marked }
        this.markedComments.replace(oldComment, newComment)

        Store.dispatch('toggleMarkComment', { id: comment.id, note: '' })
    }
}

const template = /* html */ `
<!-- ko ifnot: isLoading -->

<h1 class="display-4 mt-0" data-bind="text: profile().displayName">User Name here</h1>

<div class="card">
    <div class="card-body">
        <p><b>Email: </b><span data-bind="text: profile().email"></span></p>
        <p><b>Profile created: </b><span data-bind="text: profile().creationDate"></span></p>
    </div>
    <ul class="list-group list-group-flush">
        <li class="list-group-item bg-light">
            <b>Search history:</b>
        </li>
        <!-- ko foreach: searchHistory -->
        <li class="list-group-item" data-bind="text: $data.searchText"></li>
        <!-- /ko -->
    </ul>
</div>

<h1 class="display-4 mb-2 mt-5">Marked posts</h1>
<!-- ko foreach: markedPosts -->
<div class="mb-5" data-bind="component: {
    name: 'so-post',
    params: {
        post: $data,
        toggle: $component.togglePost,
        showLink: true
    }
}"></div>
<!-- /ko -->

<h1 class="display-4 mb-2 mt-5">Marked comments</h1>
<ul class="list-group mx-3 mx-lg-auto" data-bind="foreach: markedComments">
    <li class="list-group-item">
        <aside class="float-left pr-3">
            <div class="card-star" data-bind="click: (comment, event) => $component.toggleComment(comment)">
                <i class="fas fa-star" data-bind="css: { fas: $data.marked, far: !$data.marked }"></i>
                <div class="card-star-title">mark</div>
            </div>
            <div class="card-score">
                <div class="card-score-count" data-bind="text: $data.score"></div>
                <div class="card-score-title">score</div>
            </div>
        </aside>
        <p class="mb-1 clearfix" data-bind="text: $data.text"></p>
        <!-- ko if: $data.note -->
        <p class="mt-2 text-info font-weight-bold" data-bind="text: 'Note: ' + $data.note"></p>
        <!-- /ko -->
        <footer class="text-muted text-right small">
            <span>by</span>
            <cite data-bind="text: $data.authorDisplayName"></cite>
            <span>on the <time data-bind="text: $data.creationDate"></time></span>
        </footer>
    </li>
</ul>
<!-- /ko -->
`

export default { viewModel: UserProfile, template }