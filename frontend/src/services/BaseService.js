export class BaseService {
    constructor(baseUrl, requestOptions = {}) {
        this.baseUrl = baseUrl

        const token = localStorage.getItem('token')

        if (token) {
            this.requestOptions = {
                credentials: 'include',
                headers: {
                    'Authorization': `Bearer ${token}`
                },
                ...requestOptions
            }
        } else {
            this.requestOptions = requestOptions
        }
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