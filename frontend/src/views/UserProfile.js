import Store from '@/Store.js'
import { observable, observableArray, computed } from 'knockout'
import { Component, wrapComponent } from '../components/Component.js'
import UserService from '../services/UserService.js'

class UserProfile extends Component {
    constructor(args) {
        super(args)

        this.isLoading(true)

        this.profile = observable({});
        this.markedPosts = observableArray();
        this.markedComments = observableArray();

        this.fetchUserProfile()
    }

    async fetchUserProfile() {
        let result = await UserService.getUserProfile()

        this.profile(result)
        this.markedPosts(result.markedPosts)
        this.markedComments(result.markedComments)
        this.isLoading(false)
    }

    async unmarkPost(post, event) {
        event.preventDefault()
        this.markedPosts.destroy(post)
        await Store.dispatch('toggleMarkPost', post.id)
    }

    async unmarkComment(comment, event) {
        event.preventDefault()
        this.markedComments.destroy(comment)
        await Store.dispatch('toggleMarkComment', comment.id)
    }
}

const template = /* html */ `
<div data-bind="visible: !isLoading()">
    <so-loader id="loader" data-bind="visible: isLoading()" params="size: 200"></so-loader>
    <div class="card-user-profile">
        <div>
            <h2 class="display-4" data-bind="text: profile().displayName">User Name here</h2>
            <p><strong>Email: </strong><span data-bind="text: profile().email"></span> </p>
            <p><strong>Profile created: </strong><span data-bind="text: profile().creationDate"></span> </p>
        </div>
    </div>

    <h2 class="display-4">Marked Posts</h2>
    <div class="card">
        <ul class="list-group list-group-flush" data-bind="foreach: markedPosts">
            <li class="list-group-item d-flex justify-content-between align-items-center py-4">
                <div class="mr-3 text-center flex-shrink-1">
                    <span class="d-block badge badge-primary badge-pill" data-bind="text: $data.score"></span>
                    <small class="d-block text-muted">score</small>
                </div>
                <article class="flex-grow-1">
                    <span class="profile-note" data-bind="visible: $data.note, text: 'Note: ' + $data.note"></span>

                    <h5 class="card-title" data-bind="visible: $data.title, text: $data.title"></h5>

                    <div data-bind="text: $data.body"></div>

                    <cite class="d-block mt-3" data-bind="attr: { title: $data.authorDisplayName }">
                        <span class="text-muted" data-bind="text: ' - ' + $data.authorDisplayName"></span>
                    </cite>
                </article>
                <div>
                <i class="fas fa-star fa-2x marked-star" data-bind="click: (post, event) => $component.unmarkPost(post, event)"></i>
                </div>
            </li>
        </ul>
    </div>

    <h2 class="display-4 profile-title">Marked Comments</h2>
    <div class="card">
        <ul class="list-group list-group-flush" data-bind="foreach: markedComments">
            <li class="list-group-item d-flex justify-content-between align-items-center py-4">
                <div class="mr-3 text-center flex-shrink-1">
                    <span class="d-block badge badge-primary badge-pill" data-bind="text: $data.score"></span>
                    <small class="d-block text-muted">score</small>
                </div>
                <article class="flex-grow-1">
                    <span class="profile-note" data-bind="visible: $data.note, text: 'Note: ' + $data.note"></span>

                    <div data-bind="text: $data.text"> </div>

                    <cite class="d-block mt-3" data-bind="attr: { title: $data.authorDisplayName }">
                        <span class="text-muted" data-bind="text: ' - ' + $data.authorDisplayName"></span>
                    </cite>
                </article>

                <i class="fas fa-star fa-2x marked-star" data-bind="click: (comment, event) => $component.unmarkComment(comment, event)"></i>
            </li>
        </ul>
    </div>
</div>
`

export default wrapComponent(UserProfile, template)