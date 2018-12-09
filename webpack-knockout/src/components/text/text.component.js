import Store from '../../store.js'
import { computed } from 'knockout'

export class TextComponent {
    constructor() {
        this.testData = computed({
            read() {
                return Store.state().testData
            },
            write(testData) {
                Store.update({ testData })
            }
        })
    }
}