import Store from '@/Store.js'
import Router from '@/Router.js'
import { observable } from 'knockout'

class PostList {
    constructor(params = {}) {
        // this.searchParams = Store.getters.searchParams
        // this.isAuthenticated = Store.getters.isAuthenticated
        this.items = params.items
        console.log('postList - param', params)
        console.log('postList-items', this.items)
        //
        // this.showNote = observable(false)
        // this.note = observable('')
        // this.focusedPost
        //
        // this.postMarker = document.getElementById('post-marker')
    }

    // navigate(event, routeName, params = {}) {
    //     event.preventDefault()
    //     Router.setRoute(routeName, params)
    // }
    //
    // displayNoteOrToggle(data, event) {
    //     this.focusedPost = data
    //
    //     if (data.marked) {
    //         this.toggleMarkPost()
    //     } else {
    //         this.showNote(true)
    //
    //         const { x, y } = event.srcElement.getBoundingClientRect()
    //
    //         this.postMarker.style.top = `${Math.abs(y - 60)}px`
    //         this.postMarker.style.left = `${Math.abs(x - 260)}px`
    //     }
    // }
    //
    // toggleMarkPost() {
    //     const newItems = this.items.peek().map(post => {
    //         if (post.id === this.focusedPost.id) {
    //             const marked = !post.marked
    //             return { ...post, marked, note: marked ? this.note() : '' }
    //         }
    //
    //         return post
    //     })
    //
    //     this.showNote(false)
    //     this.items(newItems)
    //     Store.dispatch('toggleMarkPost', { id: this.focusedPost.id, note: this.note() })
    // }
}

const template = /* html */ `
<!-- ko foreach: items -->
 <section data-bind="component: { name: 'so-post', params: { post: $data } } "></section>
 <!--<section data-bind="text: $data.score"></section>-->
<!-- /ko -->
`

export default { viewModel: PostList, template }