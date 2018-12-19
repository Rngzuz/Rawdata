import Store from '@/Store.js'
import QuestionService from 'Services/QuestionService.js'
import { observable, observableArray } from 'knockout'

class Question {
    constructor({ id }) {
        this.postId = id
        this.isLoading = Store.getters.isLoading
        this.post = observable({})
        this.answers = observableArray([])

        this.fetchQuestion()
        this.toggleQuestion = this.toggleQuestion.bind(this)
        this.toggleAnswer = this.toggleAnswer.bind(this)
    }

    async fetchQuestion() {
        this.isLoading(true)

        const { answers, ...post } = await QuestionService
            .fetchQuestionById(this.postId)

        this.post(post)
        this.answers(answers)

        this.isLoading(false)
    }

    toggleQuestion(post) {
        this.post({
            ...post,
            marked: !post.marked
        })

        Store.dispatch('toggleMarkPost', { id: post.id, note: '' })
    }

    toggleAnswer(post) {
        const oldAnswer = this.answers.peek()
            .find(answer => answer.id === post.id)

        const newAnswer = { ...oldAnswer, marked: !oldAnswer.marked }
        this.answers.replace(oldAnswer, newAnswer)

        Store.dispatch('toggleMarkPost', { id: post.id, note: '' })
    }
}

const template = /* html */ `
<!-- ko ifnot: isLoading -->
<div data-bind="component: {
    name: 'so-post',
    params: {
        post: post(),
        toggle: toggleQuestion,
        showComments: true
    }
}"></div>
<!-- ko foreach: answers -->
<div class="mt-5" data-bind="component: {
    name: 'so-post',
    params: {
        post: $data,
        toggle: $component.toggleAnswer,
        showComments: true
    }
}"></div>
<!-- /ko -->
<!-- /ko -->
`

export default { viewModel: Question, template }