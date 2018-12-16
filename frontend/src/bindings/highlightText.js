import { unwrap } from 'knockout'

const getSentences = text => {
    const element = document.createElement('div')
    element.innerHTML = text

    return Array
        .from(element.children)
        .filter(tag => !/(hr|br|img)/i.test(tag.tagName))
        .map(tag => {
            return tag.innerHTML
                .replace(/<(\/|).+?>/g, '')
                .replace(/(^[\s\:\;]+|[\s\:\;]+$)/g, '')
                .split(/[\.\?\:!]\s+?/g)
        })
        .flatMap(strings => [...strings])
}

const getRelevantSentences = (texts, toMark, joinBy) => {
    const regex = new RegExp(`\\b(${toMark.join('|')})\\b`, 'gi')

    return texts
        .filter(string => regex.test(string))
        .join(joinBy)
        .replace(/[<>]/g, i => '&#' + i.charCodeAt(0) + ';')
        .replace(regex, '<mark>$&</mark>')
}

const stripHtmlAndMark = (text, toMark) => {
    const regex = new RegExp(`\\b(${toMark.join('|')})\\b`, 'gi')

    return text
        .replace(/[<>]/g, i => '&#' + i.charCodeAt(0) + ';')
        .replace(regex, '<mark>$&</mark>')
}

const getPlainExcerpt = (text, length = 400) => {
    const element = document.createElement('div')
    element.innerHTML = text

    return Array
        .from(element.children)
        .filter(tag => /(p|code)/i.test(tag.tagName))
        .map(tag => tag.textContent)
        .join(' ')
        .substring(0, length)

}

const getMarkedExcerpt = (text, toMark, joinBy = ' ... ') =>
    getRelevantSentences(getSentences(text), toMark, joinBy)

export { getPlainExcerpt, getMarkedExcerpt, stripHtmlAndMark }

// export default {
//     init(element, valueAccessor, allBindings) {
//         // Get value passed to the binding and unwrap the observable
//         const text = unwrap(valueAccessor())

//         // Get parameters passed to the binding
//         const units = allBindings.get('units')

//         if (text) {
//             // Replace all angle brackets with their character code
//             const encodedText = text
//                 .replace(/[<>]/gmi, i => '&#' + i.charCodeAt(0) + ';')

//             if (units && units.length > 0) {
//                 // Match the words and insert them wrapped in the <mark> tag
//                 const regex = new RegExp(`\\b(${units.join('|')})\\b`, 'gmi')

//                 element.innerHTML =
//                     encodedText.replace(regex, '<mark>$&</mark>')
//                     + element.innerHTML
//             } else {
//                 // Insert encoded text
//                 element.innerHTML =
//                     encodedText
//                     + element.innerHTML
//             }
//         }
//     }
// }