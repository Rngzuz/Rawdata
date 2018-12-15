import * as d3 from 'd3'
import cloud from 'd3-cloud'

import { Component } from './Component.js'
import SearchService from 'Services/SearchService.js'

class WordCloud extends Component {
    constructor(args) {
        super(args)

        if (this.$params.word) {
            this.loadGraphInput(this.$params.word)
        }
    }

    async loadGraphInput(word) {
        let result = await SearchService.getWords(word, 100)
        this.drawWordCloud(result)
    }

    drawWordCloud(result) {
        const words = result.map(item => ({
            text: item.word,
            size: item.weight
        }))

        const layout = cloud()
            .size([1110, 500])
            .words(words)
            .padding(5)
            .rotate(() => ~~(Math.random() * 2) * 90)
            .font('Impact')
            .fontSize(d => d.size)
            .start()

        d3.select(this.$el.querySelector('svg'))
            .append('g')
            .attr('transform', `translate(${layout.size()[0] / 2},${layout.size()[1] / 2})`)
            .selectAll('text')
            .data(words)
            .enter()
            .append('text')
            .style('font-size', d => d.size + 'px')
            .style('font-family', 'Impact')
            .attr('text-anchor', 'middle')
            .attr('transform', d => `translate(${[d.x, d.y]})rotate(${d.rotate})`)
            .text(d => d.text)
    }
}

const template = /* html */ `
<svg data-bind="visible: $params.word" width="1110" height="500"></svg>
`

export default {
    viewModel: {
        createViewModel: (...args) => new WordCloud(args)
    },
    template
}