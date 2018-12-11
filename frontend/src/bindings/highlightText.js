import { unwrap } from 'knockout'

export default {
    init(element, valueAccessor, allBindings) {
        const value = unwrap(valueAccessor())
        const words = allBindings.get('units')

        if (value && value.length > 0 && words && words.length > 0) {
            const group = words.map(word => `\\b${word}\\b`).join('|')
            const regex = new RegExp(`(${group})\\b`, 'gi')

            element.innerHTML = value.replace(regex, '<mark>$&</mark>')
        }
    }
}