const domParser = new DOMParser()

const getSentences = text => {
    const doc = domParser
        .parseFromString(text, 'text/html')

    return Array
        .from(doc.body.children)
        .filter(tag => !/(hr|br|img)/i.test(tag.tagName))
        .map(tag => {
            return tag.innerHTML
                .replace(/<(\/|).+?>/g, '')
                .replace(/(^[\s\:\;]+|[\s\:\;]+$)/g, '')
                .split(/[\.\?\:!]\s+?/g)
        })
        .flatMap(strings => [...strings])
}

const getRelevantSentences = (texts, toMark, joinBy, length = 400) => {
    const regex = new RegExp(`\\b(${toMark.join('|')})\\b`, 'gi')

    return texts
        .filter(string => regex.test(string))
        .join(joinBy)
        .replace(/[<>]/g, i => '&#' + i.charCodeAt(0) + ';')
        .substring(0, length)
        .replace(regex, '<mark>$&</mark>')
}

const escapeHtml = text => {
    return text.replace(/[<>]/g, i => '&#' + i.charCodeAt(0) + ';')
}

const escapeHtmlAndMark = (text, toMark) => {
    const regex = new RegExp(`\\b(${toMark.join('|')})\\b`, 'gi')

    return escapeHtml(text)
        .replace(regex, '<mark>$&</mark>')
}

const getPlainExcerpt = (text, length = 400) => {
    const doc = domParser
        .parseFromString(text, 'text/html')

    return Array
        .from(doc.body.children)
        .filter(tag => /(p|code)/i.test(tag.tagName))
        .map(tag => tag.textContent)
        .join(' ')
        .replace(/[<>]/g, i => '&#' + i.charCodeAt(0) + ';')
        .substring(0, length)

}

const getMarkedExcerpt = (text, toMark, joinBy = ' ... ') =>
    getRelevantSentences(getSentences(text), toMark, joinBy)

export { getPlainExcerpt, getMarkedExcerpt, escapeHtmlAndMark, escapeHtml }
