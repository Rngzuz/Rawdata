class SearchService {
    constructor() {
        this.baseUrl = 'http://localhost:5000/api/search'
    }

    async getNewest() {
        const response = await fetch(`http://localhost:5000/api/questions`)

        return await response.json()
    }

    async getBestMatch(words, page = 1, size = 50) {
        const response = await fetch(
            this.buildQuery('best', words, page, size),
            {
                credentials: 'include',
                headers: {
                    'Authorization': `Bearer ${localStorage.getItem('token')}`
                }
            }
        )

        return await response.json()
    }

    async getExactMatch(words, page = 1, size = 50) {
        const response = await fetch(
            this.buildQuery('exact', words, page, size)
        )

        return await response.json()
    }

    async getRankedWeightedMatch(words, page = 1, size = 50) {
        const response = await fetch(
            this.buildQuery('ranked', words, page, size)
        )

        return await response.json()
    }

    async getWords(word, size = 100) {
        const response = await fetch(`${this.baseUrl}/words?word=${word}&size=${size}`)

        return await response.json()
    }

    async getWordAssociation(word, size = 100) {
        const response = await fetch(`${this.baseUrl}/context?word=${word}&size=${size}`)

        return await response.json()
    }

    buildQuery(path, words, page, size) {
        const queryString =
            words.map(value => `words=${value}`).join('&')
            + `&page=${page}`
            + `&size=${size}`

        return `${this.baseUrl}/${path}?${queryString}`
    }
}

export default new SearchService()