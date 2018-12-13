import { BaseService } from './BaseService.js'

class SearchService extends BaseService {
    constructor() {
        super('/api')
    }

    async getNewest() {
        const endpoint = this.buildUrl({ path: '/questions' })
        const response = await fetch(endpoint)

        return await response.json()
    }

    async getBestMatch(words, page = 1, size = 50) {
        const endpoint = this.buildUrl({
            path: '/search/best',
            searchParams: { words, page, size }
        })

        const response = await fetch(endpoint, this.requestOptions)

        return await response.json()
    }

    async getExactMatch(words, page = 1, size = 50) {
        const endpoint = this.buildUrl({
            path: '/search/exact',
            searchParams: { words, page, size }
        })

        const response = await fetch(endpoint, this.requestOptions)

        return await response.json()
    }

    async getRankedWeightedMatch(words, page = 1, size = 50) {
        const endpoint = this.buildUrl({
            path: '/search/ranked',
            searchParams: { words, page, size }
        })

        const response = await fetch(endpoint, this.requestOptions)

        return await response.json()
    }

    async getWords(word, size = 100) {
        const endpoint = this.buildUrl({
            path: '/search/words',
            searchParams: { word, size }
        })

        const response = await fetch(endpoint, this.requestOptions)

        return await response.json()
    }

    async getWordAssociation(word, size = 100) {
        const endpoint = this.buildUrl({
            path: '/search/words',
            searchParams: { word, size }
        })

        const response = await fetch(endpoint, this.requestOptions)

        return await response.json()
    }

    async getForceGraphInput(word, grade = 8) {
        const endpoint = this.buildUrl({
            path: '/search/forcegraph',
            searchParams: { word, grade }
        })

        const response = await fetch(endpoint, this.requestOptions)

        return await response.json()
    }
}

export default new SearchService()