import { Component, wrapComponent } from 'Components/Component.js'

class SignIn extends Component {
    constructor(args) {
        super(args)
    }

    signIn() {
        this.$store.dispatch('updateIsAuthenticated', true)
        this.$router.setRoute('home')
    }
}

const template = /* html */ `
<div class="card">
    <div class="card-body">
        <form data-bind="submit: () => signIn()">
            <div class="form-group">
                <label for="email">Email address</label>
                <input type="email" class="form-control" id="email" placeholder="Enter email">
                <small class="form-text text-muted">We'll never share your email with anyone else.</small>
            </div>
            <div class="form-group">
                <label for="password">Password</label>
                <input type="password" class="form-control" id="password" placeholder="Password">
            </div>
            <button type="submit" class="btn btn-primary">Sign in</button>
        </form>
    </div>
</div>
`


export default wrapComponent(SignIn, template)