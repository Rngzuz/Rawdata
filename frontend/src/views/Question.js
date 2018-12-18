import { observable } from 'knockout'
import QuestionService from 'Services/QuestionService.js'
import { Component, wrapComponent } from 'Components/Component.js'

class Question extends Component {
    constructor(args) {
        super(args)
        this.question = observable({})
        this.answers = observable([])

        this.fetchQuestion()
    }

    async fetchQuestion() {
        this.isLoading(true)
        // const response = await fetch(`http://localhost:5000/api/questions/${this.$params.id}`)
        const response = await QuestionService.fetchQuestionById(this.$params.id)
        this.question(response)

        console.log('fetch question', this.question())
        this.answers(this.question().answers)
        this.isLoading(false)
    }
}

const template = /* html */ `
<!-- ko if: !isLoading() -->
<section>
    <!--<h2 class="mt-5 mb-3" data-bind="text: question().title"></h2>-->

    <section data-bind="component: { name: 'so-post', params: { post: question } } "></section>

    <div class="ml-lg-3">
            <h5>Answer</h5>
            <section data-bind="component: { name: 'so-post-list', params: { items: answers } } "></section>
       </div>

    <!--&lt;!&ndash; ko foreach: question().answers &ndash;&gt;-->
        <!--<div class="card mb-5">-->
            <!--<div class="card-body" data-bind="html: $data.body"></div>-->

            <!--<footer class="card-footer text-muted">-->
                <!--<cite data-bind="text: $data.authorDisplayName"></cite>-->
            <!--</footer>-->

            <!--&lt;!&ndash;<ul class="list-group list-group-flush" data-bind="foreach: $data.comments">&ndash;&gt;-->
                <!--&lt;!&ndash;<li class="list-group-item" data-bind="html: $data.text"></li>&ndash;&gt;-->
            <!--&lt;!&ndash;</ul>&ndash;&gt;-->
            <!--<h5>Comments</h5>-->
            <!--<section data-bind="component: { name: 'so-comment-list', params: { items: $data.comments } } "></section>-->
        <!--</div>-->
    <!--&lt;!&ndash; /ko &ndash;&gt;-->
</section>
<!-- /ko -->
`

export default wrapComponent(Question, template)