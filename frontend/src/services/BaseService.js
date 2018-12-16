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

        //
        // this.requestOptions = {
        //     credentials: 'include',
        //     headers: {
        //         'Authorization': `Bearer eyJhbGciOiJIUzM4NCIsInR5cCI6IkpXVCJ9.eyJpZCI6IjMiLCJ1bmlxdWVfbmFtZSI6IlRlc3RVc2VyIiwiZW1haWwiOiJ0ZXN0QHRlc3QuY29tIiwibmJmIjoxNTQ0OTU4NDcwLCJleHAiOjE1NDc2MzY4NzAsImlhdCI6MTU0NDk1ODQ3MH0.5G23k-KGqbDEbmMnsgLjMBepju6em7wqomes0O3jNO6mmO2iDtt-96GuYfLzY1wS`
        //         // 'Authorization': `Bearer ${token}`
        //     },
        //     ...requestOptions
        // }
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