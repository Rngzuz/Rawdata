import Store from '@/Store.js'
import { observable } from 'knockout'
import { Component } from './Component.js'

class Navbar extends Component {
    constructor(args) {
        super(args)
        this.rawSearch = observable('')
        this.collapsible = this.$el.querySelector('#collapse')
    }

    updateSearch() {
        const rawSearch = this.rawSearch()
        Store.dispatch('updateSearchParams', rawSearch)
    }

    toggleCollapse() {
        this.collapsible.classList.toggle('show')
    }

    setCurrentView(event, newView) {
        event.preventDefault()
        this.$params.currentView(newView)
    }
}

const template = /* html */ `
<nav class="navbar fixed-top navbar-expand-lg navbar-light bg-light">
    <div class="container">
        <a class="navbar-brand" href="#">App</a>
        <button class="navbar-toggler" type="button" data-bind="click: toggleCollapse">
            <span class="navbar-toggler-icon"></span>
        </button>
        <div id="collapse" class="collapse navbar-collapse">
            <ul class="navbar-nav mr-auto">
                <li class="nav-item">
                    <a class="nav-link" href="/home" data-bind="click: (_, event) => setCurrentView(event, 'so-home')">Home</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link" href="/about" data-bind="click: (_, event) => setCurrentView(event, 'so-sign-in')">Sign in</a>
                </li>
            </ul>

            <form class="form-inline my-2 my-lg-0" data-bind="submit: updateSearch">
                <input type="text" class="form-control" placeholder="Search..." data-bind="value: rawSearch">
            </form>
        </div>
    </div>
</nav>
`

export default {
    viewModel: {
        createViewModel: (...args) => new Navbar(args)
    },
    template
}