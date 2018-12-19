import { Component, wrapComponent } from 'Components/Component.js'
import AuthService from '../services/AuthService.js'
import { observable } from 'knockout'

class SignIn extends Component {
    constructor(args) {
        super(args)

        this.email = observable("")
        this.password = observable("")
    }

    async signIn() {
        if (!this.inputFilledOut()) {
            return
        }

        const credentials = { "email": this.email(), "password": this.password() }

        let response = await AuthService.signIn(credentials)

        if (response !== undefined) {
            this.$store.dispatch('updateIsAuthenticated', true)
            this.$router.setRoute('home')

        } else {
            alert('Sign in failed')
        }
    }

    inputFilledOut() {
        return this.email().length > 0 && this.password().length > 0
    }
}

const template = /* html */ `
<div class="card">
    <div class="card-body">
        <form data-bind="submit: signIn">
            <div class="form-group">
                <label for="email">Email address</label>
                <input type="email" class="form-control" id="email" data-bind="value: email" required>
                <small class="form-text text-muted">We'll never share your email with anyone else.</small>
            </div>
            <div class="form-group">
                <label for="password">Password</label>
                <input type="password" class="form-control" id="password" data-bind="value: password" required pattern=".{8,}" title="Password should be atleast 8 characters long">
            </div>
            <button type="submit" class="btn btn-primary">Sign in</button>
        </form>
    </div>
</div>
`


export default wrapComponent(SignIn, template)