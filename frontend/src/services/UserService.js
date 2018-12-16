import { BaseService } from './BaseService.js'

class UserService extends BaseService {
    constructor() {
        super('/api/users')
    }

    async getUserProfile() {
        const endpoint = this.buildUrl({ path: '/profile' })
        const response = await fetch(endpoint, this.requestOptions)

        return await response.json()
    }

}

export default new UserService()