import Store from '@/Store'
import Router from '@/Router.js'
import { computed } from 'knockout'

export class Component {
    constructor(args) {
        if (args[0] !== undefined) {
            const { $raw = {}, ...params } = args[0]

            this.$raw = $raw
            this.$params = params
        } else {
            this.$raw = {}
            this.$params = {}
        }

        if (args[1] !== undefined) {
            const { element = undefined, templateNodes = [] } = args[1]

            this.$el = element
            this.$tNodes = templateNodes
        } else {
            this.$el = undefined
            this.$tNodes = []
        }

        this.$store = Store
        this.$router = Router

        this.isLoading = computed({
            read: this.$store.getters.isLoading,
            write: newValue => this.$store.dispatch('updateIsLoading', newValue)
        })
    }
}

export function wrapComponent(viewModel, template) {
    return {
        viewModel: {
            createViewModel: (...args) => new viewModel(args)
        },
        template
    }
}
