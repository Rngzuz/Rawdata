import Store from '@/Store.js'
import Router from '@/Router.js'
import AuthService from 'Services/AuthService.js'
import { observable } from 'knockout'
import { Component, wrapComponent } from './Component.js'

class Navbar extends Component {
    constructor(args) {
        super(args)
        this.rawSearch = observable('')
        this.collapsible = document.getElementById('navbarCollapse')
    }

    updateSearch() {
        const rawSearch = this.rawSearch()
        // Router.setRoute('home')
        Store.dispatch('updateSearchParams', rawSearch)
    }

    toggleCollapse() {
        this.collapsible.classList.toggle('show')
    }

    navigate(event, routeName, params = {}) {
        event.preventDefault()
        Router.setRoute(routeName, params)
    }

    signOut() {
        AuthService.signOut()
        Store.commit('SET_IS_AUTHENTICATED', false)
    }
}

const template = /* html */ `
<nav class="navbar fixed-top navbar-expand-lg navbar-light bg-light">
    <div class="container">
        <a class="navbar-brand" href="#">App</a>
        <button class="navbar-toggler" type="button" data-bind="click: toggleCollapse">
            <span class="navbar-toggler-icon"></span>
        </button>
        <div id="navbarCollapse" class="collapse navbar-collapse">
            <ul class="navbar-nav mr-auto">
                <li class="nav-item">
                    <a class="nav-link" href="/home" data-bind="click: (_, event) => navigate(event, 'home')">Home</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link" href="/force-Graph" data-bind="click: (_, event) => navigate(event, 'force-graph')">Force Graph</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link" href="/word-cloud" data-bind="click: (_, event) => navigate(event, 'word-cloud')">Word Cloud</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link" href="/user-profile" data-bind="visible: $store.getters.isAuthenticated(), click: (_, event) => navigate(event, 'user-profile')">User profile</a>
                </li>
            </ul>

            <form class="form-inline my-2 mr-0 my-lg-0 mr-lg-2" data-bind="submit: updateSearch">
                <input type="text" class="form-control" placeholder="Search..." data-bind="value: rawSearch">
            </form>

            <button class="btn btn-outline-success mr-2"
                data-bind="visible: !$store.getters.isAuthenticated(), click: (_, event) => navigate(event, 'sign-in')"
                type="button">Sign in</button>

            <button class="btn btn-outline-danger"
                data-bind="visible: $store.getters.isAuthenticated(), click: signOut"
                type="button">Sign Out</button>

            <button class="btn btn-outline-primary"
                data-bind="visible: !$store.getters.isAuthenticated(), click: (_, event) => navigate(event, 'register')"
                type="button">Register</button>
        </div>
    </div>
</nav>
`

export default wrapComponent(Navbar, template)