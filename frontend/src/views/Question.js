import { observable } from 'knockout'
import { Component, wrapComponent } from 'Components/Component.js'

class Question extends Component {
    constructor(args) {
        super(args)
        this.question = observable({
            title: '',
            authorDisplayName: '',
            body: '',
            answers: []
        })

        this.fetchQuestion()
    }

    async fetchQuestion() {
        this.isLoading(true)
        const response = await fetch(`http://localhost:5000/api/questions/${this.$params.id}`)
        this.question(await response.json())
        this.isLoading(false)
    }
}

const template = /* html */ `
<section data="visible: !isLoading">
    <h2 class="mt-5 mb-3" data-bind="text: question().title"></h2>

    <article class="card mb-5">
        <section class="card-body" data-bind="html: question().body"></section>
        <footer class="card-footer text-muted">
            <cite data-bind="text: question().authorDisplayName"></cite>
        </footer>
    </article>

    <!-- ko foreach: question().answers -->
        <div class="card mb-5">
            <div class="card-body" data-bind="html: $data.body"></div>

            <footer class="card-footer text-muted">
                <cite data-bind="text: $data.authorDisplayName"></cite>
            </footer>

            <ul class="list-group list-group-flush" data-bind="foreach: $data.comments">
                <li class="list-group-item" data-bind="html: $data.text"></li>
            </ul>
        </div>
    <!-- /ko -->
</section>
`

export default wrapComponent(Question, template)