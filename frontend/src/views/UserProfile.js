import Store from '@/Store.js'
import { observable, observableArray, computed } from 'knockout'
import { Component, wrapComponent } from '../components/Component.js'
import UserService from '../services/UserService.js'
import {escapeHtml, getPlainExcerpt} from "@/bindings/highlightText"

class UserProfile extends Component {
    constructor(args) {
        super(args)

        this.isLoading(true)

        this.profile = observable({});
        this.markedPosts = observableArray([]);
        this.markedComments = observableArray([]);

        this.fetchUserProfile()
    }

    async fetchUserProfile() {
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

        // result.markedComments = result.markedComments.map(comment => {
        //     const endIndex = comment.length > 200 ? 200 : comment.length
        //
        //     return {
        //         ...comment,
        //         text: comment.text.substring(0, endIndex) + "..."
        //     }
        // })

        this.profile(result)
        this.markedPosts(result.markedPosts)
        this.markedComments(result.markedComments)
        this.isLoading(false)

    }
}

const template = /* html */ `
<div data-bind="visible: !isLoading()">
    <div class="card-user-profile">
        <div>
            <h2 class="display-4" data-bind="text: profile().displayName">User Name here</h2>
            <p><strong>Email: </strong><span data-bind="text: profile().email"></span> </p>
            <p><strong>Profile created: </strong><span data-bind="text: profile().creationDate"></span> </p>
        </div>
    </div>

    <h2 class="display-4">Marked Posts</h2>
    <section data-bind="component: { name: 'so-list', params: { items: markedPosts } } "></section>

    <h2 class="display-4 profile-title">Marked Comments</h2>
    <section data-bind="component: { name: 'so-comment-list', params: { items: markedComments } } "></section>
</div>
`

export default wrapComponent(UserProfile, template)