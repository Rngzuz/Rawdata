import ko from 'knockout'
import Store from './Store.js'

// Stylesheets
import 'bootstrap/dist/css/bootstrap.min.css'
import './style.css'

// Custom bindings
import highlightText from './bindings/highlightText.js'

// Views
import Home from 'Views/Home.js'
import SignIn from 'Views/SignIn.js'
import UserProfile from 'Views/UserProfile.js'

// Components
import App from './App.js'
import Loader from 'Components/Loader.js'
import Navbar from 'Components/Navbar.js'
import ForceGraph from 'Components/ForceGraph.js'
import Search from 'Components/Search.js'
import WordCloud from 'Components/WordCloud.js'

ko.components.register('so-home', Home)
ko.components.register('so-sign-in', SignIn)
ko.components.register('so-user-profile', UserProfile)

ko.components.register('so-app', App)
ko.components.register('so-loader', Loader)
ko.components.register('so-navbar', Navbar)
ko.components.register('so-force-graph', ForceGraph)
ko.components.register('so-search', Search)
ko.components.register('so-word-cloud', WordCloud)

ko.bindingHandlers.highlightText = highlightText

ko.applyBindings()
