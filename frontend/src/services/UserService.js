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

    async toggleMarkedPost(postId, note) {
        const endpoint = this.buildUrl({path: 'posts'})
        const response = await fetch(endpoint, {
            method: 'POST',
            body: {"postId": postId, "note": note},
            headers: { 'Content-Type': 'application/json' }
        })

        return await response.json()
    }

    async updateMarkedPostNote(postId, note) {
        const endpoint = this.buildUrl({path: 'posts/'+postId})
        const response = await fetch(endpoint, {
            method: 'POST',
            body: {"postId": postId, "note": note},
            headers: { 'Content-Type': 'application/json' }
        })

        return await response.json()
    }

}

export default new UserService()