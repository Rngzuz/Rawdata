import Store from '../../store.js'
import { pureComputed } from 'knockout'

export class TestComponent {
    constructor() {
        this.testData = pureComputed({
            read() {
                return Store.state().testData
            }
        })
    }
}