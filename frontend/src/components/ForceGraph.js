import * as echarts from 'echarts'
import * as randomColor from 'randomcolor'
import { Component } from './Component.js'
import SearchService from '@/services/SearchService'
import { wrapComponent } from '@/components/Component'

class ForceGraph extends Component {
    constructor(args) {
        super(args)

        this.chart = echarts.init(document.getElementById('chart'))
        // this.initGraph()
        this.initCircularGraph()

        this.$store.subscribe('searchParams', value => {
            this.isLoading(true)
            this.fetchGraphInput(value[0])
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


    async fetchGraphInput(word) {

        let result = await SearchService.getForceGraphInput(word)
        this.drawForceGraph(result, word)
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
                type: 'graph',
                layout: 'force',
                roam: true,
                focusNodeAdjacency: true,
                label: {
                    show: true,
                    normal: {
                        position: 'right'
                    }
                },
                force: {
                    repulsion: 100,
                    gravity: 0.05,
                    edgeLength: 100,

                },
                zoom: 2
            }]
        })
    }

    initCircularGraph() {
        this.chart.setOption({
            title: {
                subtext: '',
                top: 'top',
                left: 'right'
            },
            tooltip: {},
            series: [{
                type: 'graph',
                layout: 'none',
                roam: true,
                focusNodeAdjacency: true,
                label: {
                    show: true,
                    normal: {
                        position: 'right'
                    }
                },
                lineStyle: {
                    color: 'source',
                    curveness: 0.3
                },
                emphasis: {
                    lineStyle: {
                        width: 10
                    }
                },
                zoom: 0.8
            }]
        })
    }

    drawForceGraph(graphData, searchTerm) {
        this.isLoading(true)

        const nodes = this.mapNodes(graphData.nodes)
        const links = this.mapEdges(graphData.links)

        const nodesCount = nodes.length
        const edgeCount = links.length
        this.chart.setOption({
            title: {
                text: `Force Graph for '${searchTerm}'\n\nWords: ${nodesCount}\nAssociations: ${edgeCount}`,
            },
            series: [{
                data: nodes,
                links: links,
            }]
        })

        this.isLoading(false)
    }

    mapNodes(nodes) {
        return nodes.map(node => {
            node.itemStyle = {
                color: randomColor.randomColor()
            }
            node.symbolSize = 20
            node.value = node.symbolSize
            node.category = 'Word'
            node.x = node.y = null
            node.draggable = true
            node.name = node.id

            return node
        })
    }

    mapEdges(edges) {
        return edges.map(link => {
            let value = link.value % 2 > 0 ? (link.value / 2 + link.value % 2) : link.value
            let width = Math.sqrt(value)

            link.lineStyle = {
                width: width
            }

            return link
        })
    }

    dispose() {
        // Dispose chart when the component disposed
        this.chart.dispose()
    }
}

const template = /* html */ `
<div id="chart" style="width:100%;height:80vh;"></div>
`

export default wrapComponent(ForceGraph, template)