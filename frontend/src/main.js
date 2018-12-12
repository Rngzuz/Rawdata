import ko from 'knockout'

// Stylesheets
import 'bootstrap/dist/css/bootstrap.min.css'
import './style.css'

// Custom bindings
import highlightText from './bindings/highlightText.js'

// Components
import App from './App.js'
import Loader from './components/Loader.js'
import Home from './components/Home.js'
import Navbar from './components/Navbar.js'
import ForceGraph from './components/ForceGraph.js'

ko.components.register('so-app', App)
ko.components.register('so-home', Home)
ko.components.register('so-loader', Loader)
ko.components.register('so-navbar', Navbar)
ko.components.register('so-forcegraph', ForceGraph)

ko.bindingHandlers.highlightText = highlightText

ko.applyBindings()
