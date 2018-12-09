import ko from 'knockout'
import components from './components'
import 'bootstrap/dist/css/bootstrap.min.css'

components.forEach(({ name, ...component }) => {
    ko.components.register(name, component)
})

ko.applyBindings()
