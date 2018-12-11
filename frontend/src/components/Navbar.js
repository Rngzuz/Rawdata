import Store from '@/Store.js'
import { observable } from 'knockout'
import { Component } from './Component.js'

class Navbar extends Component {
    constructor(args) {
        super(args)
        this.isCollapsed = true
        this.rawSearch = observable('')
    }

    updateSearch() {
        const words = this.rawSearch().split(/\s/)
        Store.state().search(words)
    }

    toggleCollapse() {
        const elem = document.getElementById('collapse')
        elem.classList.toggle('show')
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
                    <a class="nav-link" href="/home">Home</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link" href="/about">About</a>
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