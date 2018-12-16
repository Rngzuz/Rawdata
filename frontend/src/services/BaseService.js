export class BaseService {
    constructor(baseUrl) {
        this.baseUrl = baseUrl
    }

    get requestOptions() {
        const token = localStorage.getItem('token')

        if (token) {
            return {
                credentials: 'include',
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json'
                },
                ...requestOptions
            }
        }

        return {}
    }

    objectToSearchParams(object) {
        const array = Object
            .entries(object)

        return this
            .arrayToSearchParams(array)
    }

    arrayToSearchParams(array, query = '') {
        if (array.length < 1) {
            return query ? '?' + query.substring(1) : ''
        }

        const [key, value] = array.shift()

        if (value) {
            if (Array.isArray(value)) {
                query += '&' + value
                    .map(primitive => `${key}=${encodeURIComponent(primitive)}`)
                    .join('&')
            } else {
                query += `&${key}=${encodeURIComponent(value)}`
            }
        }

        return this
            .arrayToSearchParams(array, query)
    }

    buildUrl({ path = '', searchParams = {}, fragment = '' }) {
        return this.baseUrl + path + this.objectToSearchParams(searchParams) + fragment
    }
}