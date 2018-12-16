import Store from '../Store'
import {observable, pureComputed, observableArray, computed} from 'knockout'
import {Component} from '../components/Component.js'

import UserService from '../services/UserService.js'


class UserProfile extends Component {
    constructor(args) {
        super(args)
        this.isLoading = computed({
            read: Store.getters.isLoading,
            write: newValue => Store.dispatch('updateIsLoading', newValue)
        })
        this.isLoading(true)

        this.profile = observable({});
        this.markedPosts = observableArray();
        this.markedComments = observableArray();

        this.fetchUserProfile()
    }

    async fetchUserProfile() {

        let result = await UserService.getUserProfile()
        console.log(result)

        this.profile(result)
        this.markedPosts(result.markedPosts)
        this.markedComments(result.markedComments)
        this.isLoading(false)
    }
}

const template = /* html */ `
<div data-bind="visible: !isLoading()">
    <so-loader id="loader" data-bind="visible: isLoading()" params="size: 200"></so-loader>
    <div class="card-user-profile">
            <div >
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
                    <h5 class="card-title" data-bind="visible: $data.title !== undefined">
                        <span data-bind="text: $data.title"></span>
                    </h5>
                    <div >
                        <span data-bind="text: $data.body"></span>
                    </div>
                    <cite class="d-block mt-3" data-bind="attr: { title: $data.authorDisplayName }">
                        <span class="text-muted" data-bind="text: ' - ' + $data.authorDisplayName"></span>
                    </cite>
                </article>
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
                    <div >
                        <span data-bind="text: $data.text"></span>
                    </div>
                    <cite class="d-block mt-3" data-bind="attr: { title: $data.authorDisplayName }">
                        <span class="text-muted" data-bind="text: ' - ' + $data.authorDisplayName"></span>
                    </cite>
                </article>
            </li>
        </ul>
    </div>
</div>    
`

export default {
    viewModel: {
        createViewModel: (...args) => new UserProfile(args)
    },
    template
}