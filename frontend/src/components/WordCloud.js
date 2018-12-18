import * as echarts from 'echarts'
import * as cloud from 'echarts-wordcloud'
import * as randomColor from 'randomcolor'
import {Component} from './Component.js'

import SearchService from '@/services/SearchService'
import {wrapComponent} from '@/components/Component'

class WordCloud extends Component {
    constructor(args) {
        super(args)

        this.chart = echarts.init(document.getElementById('word-cloud'))
        this.initGraph()

        this.$store.subscribe('searchParams', value => {
            this.isLoading(true)
            this.loadCloudInput(value[0])
        })

        let timeout

        window.addEventListener('resize', () => {
            if (timeout !== undefined) {
                clearTimeout(timeout)
            }
            timeout = setTimeout(() => {
                if (this.chart != null && this.chart !== undefined) {
                    this.chart.resize()
                }
            }, 300)
        })
    }

    async loadCloudInput(word) {
        let result = await SearchService.getWords(word, 100)
        this.drawWordCloud(result, word)
        console.log('result', result)
    }

    initGraph() {
        this.chart.setOption({
            title: {
                subtext: '',
                top: 'top',
                left: 'right'
            },
            tooltip: {},
            series: [{
                type: 'wordCloud',
                shape: 'circle',
                left: 'center',
                top: 'center',
                width: '100%',
                height: '100%',
                right: null,
                bottom: null,
                sizeRange: [30, 90],
                rotationRange: [-90, 90],
                rotationStep: 45,
                gridSize: 11,
                drawOutOfBound: true,
                textStyle: {
                    normal: {
                        fontFamily: 'sans-serif',
                        fontWeight: 'bold',
                        color: randomColor
                    },
                    emphasis: {
                        shadowBlur: 10,
                        shadowColor: '#333'
                    }
                },
                zoom: 2,
                // Data is an array. Each array item must have name and value property.
                data: [{
                    name: 'Enter Search Term',
                    value: 500,
                    // Style of single text
                    textStyle: {
                        normal: {},
                        emphasis: {}
                    }
                }]
            }]

        })
    }

    drawWordCloud(wordCloudData, searchTerm) {
        const words = this.mapWord(wordCloudData)

        this.chart.setOption({
            title: {
                text: `Word cloud for '${searchTerm}'`,
            },
            series: [{
                data: words,
            }]
        })
    }

    mapWord(words) {
        return words.map(word => {
            word.name = word.word
            word.value = word.weight
            word.textStyle =  {
                normal: {},
                emphasis: {}
            }
            return word
        })
    }

    dispose() {
        this.chart.dispose()
    }
}

const template = /* html */ `
<div id="word-cloud" style="width:100%;height:80vh;"></div>
`

export default wrapComponent(WordCloud, template)