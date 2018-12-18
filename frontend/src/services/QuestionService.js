import { BaseService } from './BaseService.js'

class QuestionService extends BaseService {
    constructor() {
        super('/api/questions')
    }

    async fetchQuestionById(id) {
        const endpoint = this.buildUrl({ path: `/${id}` })
        const response = await fetch(endpoint, this.requestOptions)

        return await response.json()
    }
}

export default new QuestionService()