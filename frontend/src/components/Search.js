import { Component } from './Component.js'

class Search extends Component {
    constructor(args) {
        super(args)
    }
}

const template = /* html */ `
<div class="container">
    <div class="card card-rounded-bottom border-0">
        <div class="card-body-list">hello</div>

        <div class="list-group list-group-flush">
            <a href="#" class="list-group-item list-group-item-action">Dapibus ac facilisis in</a>
            <a href="#" class="list-group-item list-group-item-action">Morbi leo risus</a>
            <a href="#" class="list-group-item list-group-item-action">Porta ac consectetur ac</a>
        </div>
    </div>
</div>
`

export default {
    viewModel: {
        createViewModel: (...args) => new Search(args)
    },
    template
}