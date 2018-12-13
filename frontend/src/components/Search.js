import { Component } from './Component.js'

class Search extends Component {
    constructor(args) {
        super(args)
    }
}

const template = /* html */ `
<div class="container">
    <div class="card">
        <div class="card-body">hello</div>
    </div>
</div>
`

export default {
    viewModel: {
        createViewModel: (...args) => new Search(args)
    },
    template
}