import * as echarts from 'echarts'
import * as randomColor from 'randomcolor'
import Store from '@/Store.js'
import SearchService from '@/services/SearchService'
import {observable} from "knockout"

class ForceGraph {
    constructor() {
        this.isLoading = Store.getters.isLoading

        this.chart = echarts.init(document.getElementById('chart'))

        this.graphType = observable('circular')

        this.searchTerm = Store.getters.searchParams

        const searchParamsSub = Store.subscribe('searchParams', value => {
            if(value[0] === undefined) {
                return
            }
            this.isLoading(true)
            this.initGraph();
            this.fetchGraphInput(value[0])
        })

        let timeout

        const chartResize = () => {
            if (timeout !== undefined) {
                clearTimeout(timeout)
            }
            timeout = setTimeout(() => {
                if (this.chart != null && this.chart !== undefined) {
                    this.chart.resize()
                }
            }, 300)
        }

        window.addEventListener('resize', chartResize)

        this.dispose = function () {
            searchParamsSub.dispose()
            window.removeEventListener('resize', chartResize)
            this.chart.dispose()
        }
    }

    searchTermEntered() {
        return this.searchTerm()[0] !== undefined
    }

    async fetchGraphInput(word) {
        let result = await SearchService.getForceGraphInput(word)
        this.drawForceGraph(result, word)
    }

    initGraph() {
        switch (this.graphType()) {
            case 'circular':
                this.initCircularGraph()
                break
            case 'force':
                this.initForceGraph()
                break
            }
    }

    initForceGraph() {
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
                layout: 'circular',
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
                text: `${this.graphType()} graph for '${searchTerm}'\n\nWords: ${nodesCount}\nAssociations: ${edgeCount}`,
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
}

const template = /* html */ `
<div class="form-inline mb-3">
        <div class="form-check">
            <label class="form-check-label">
                <input class="form-check-input" type="radio" value="circular" data-bind="checked: graphType" checked>
                <span>Circular graph</span>
            </label>
        </div>
        <div class="form-check mx-4">
            <label class="form-check-label">
                <input class="form-check-input" type="radio" value="force" data-bind="checked: graphType">
                <span>Force graph</span>
            </label>
        </div>
    </div>
<!-- placeholder -->
<!-- ko if: !searchTermEntered() -->
    <h1 class="display-4 mt-0">Enter search term</h1>
<!-- /ko -->
    
<div id="chart" style="width:100%;height:80vh;"></div>
`

export default { viewModel: ForceGraph, template }