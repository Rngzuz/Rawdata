import ko from 'knockout'

// Stylesheets
import 'bootstrap/dist/css/bootstrap.min.css'
import './style.css'

// Custom bindings
import highlightText from './bindings/highlightText.js'

// Components
import App from './App.js'
import Loader from 'Components/Loader.js'
import Home from 'Components/Home.js'
import Navbar from 'Components/Navbar.js'
import ForceGraph from 'Components/ForceGraph.js'
import Search from 'Components/Search'

ko.components.register('so-app', App)
ko.components.register('so-home', Home)
ko.components.register('so-loader', Loader)
ko.components.register('so-navbar', Navbar)
ko.components.register('so-forcegraph', ForceGraph)
ko.components.register('so-search', Search)

ko.bindingHandlers.highlightText = highlightText

ko.applyBindings()
