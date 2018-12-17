import { BaseService } from './BaseService.js'

class AuthService extends BaseService {
    constructor() {
        super('/api')
    }

    async register(user) {
        const endpoint = this.buildUrl({ path: '/register' })

        const response = await fetch(endpoint, {
            method: 'POST',
            body: JSON.stringify(user),
            headers: { 'Content-Type': 'application/json' }
        })

        return await response.json()
    }

    async signIn(credentials) {
        const endpoint = this.buildUrl({ path: '/oauth' })

        const response = await fetch(endpoint, {
            method: 'POST',
            body: JSON.stringify(credentials),
            headers: { 'Content-Type': 'application/json' }
        })

        if(response.status !== 200) {
            return undefined

        } else {

            const token = await response.text()
            localStorage.setItem('token', token)

            return token
        }
    }

    signOut() {
        localStorage.removeItem('token')
    }
}

export default new AuthService()