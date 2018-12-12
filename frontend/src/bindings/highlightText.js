import { unwrap } from 'knockout'

export default {
    init(element, valueAccessor, allBindings) {
        // Get value passed to the binding and unwrap the observable
        const text = unwrap(valueAccessor())

        // Get parameters passed to the binding
        const units = allBindings.get('units')

        if (text) {
            // Replace all angle brackets with their character code
            const encodedText = text
                .replace(/[<>]/gmi, i => '&#' + i.charCodeAt(0) + ';')

            if (units && units.length > 0) {
                // Match the words and insert them wrapped in the <mark> tag
                const regex = new RegExp(`\\b(${units.join('|')})\\b`, 'gmi')
                element.innerHTML = encodedText.replace(regex, '<mark>$&</mark>')
            } else {
                // Insert encoded text
                element.innerHTML = encodedText
            }
        }
    }
}