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
    }
}