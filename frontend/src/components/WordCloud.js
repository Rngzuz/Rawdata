import * as cloud from 'd3-cloud'
import {Canvas} from 'canvas'
import Store from '@/Store.js'
import { Component } from './Component.js'
import { pureComputed } from 'knockout'

class WordCloud extends Component {
    constructor(args) {
        super(args)
        this.isLoading = pureComputed(
            () => Store.state().isLoading()
        )

        // Store.state().search.subscribe(v => {
        //     d3.select(this.$el.querySelector('svg')).selectAll("*").remove()
        //     Store.state().isLoading(true)
        //     this.fetchGraphInput(v[0])
        // })
        this.init()
    }

    // async fetchGraphInput(word) {
    //     Store.state().isLoading(true)
    //
    //     let result = await SearchService.getForceGraphInput(word, 8)
    //     this.init(result)
    // }


    init() {

        var words = ["Hello", "world", "normally", "you", "want", "more", "words", "than", "this"]
            .map(function(d) {
                return {text: d, size: 10 + Math.random() * 90};
            });

        cloud().size([960, 500])
            .canvas(() => {return new Canvas(1, 1);})
            .words(words)
            .padding(5)
            .rotate(function() { return ~~(Math.random() * 2) * 90; })
            .font("Impact")
            .fontSize(function(d) { return d.size; })
            .start();
    }

}

const template = /* html */ `
<canvas id="canvas" width="960" height="500">
</canvas>
`

export default {
    viewModel: {
        createViewModel: (...args) => new WordCloud(args)
    },
    template
}