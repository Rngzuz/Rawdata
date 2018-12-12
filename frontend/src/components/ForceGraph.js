import * as d3 from 'd3'
import Store from '@/Store.js'
import {Component} from './Component.js'
import {pureComputed} from 'knockout'

import graphData from './ForceGraphData'

class ForceGraph extends Component {
    constructor(args) {
        super(args)
        this.isLoading = pureComputed(
            () => Store.state().isLoading()
        )

        this.graph = graphData
        this.init()
    }

    init() {
        Store.state().isLoading(true)

        const svg = d3.select(this.$el.querySelector('svg'))
        console.log(svg)
        const width = +svg.attr("width")
        const height = +svg.attr("height")

        const color = d3.scaleOrdinal(d3.schemeCategory10)

        const simulation = d3.forceSimulation()
            .force("link", d3.forceLink().id(function (d) {
                return d.id;
            }).strength(0.001))
            .force("charge", d3.forceManyBody())
            .force("center", d3.forceCenter(width / 2, height / 2))

        const link = svg.append("g")
            .attr("class", "links")
            .selectAll("line")
            .data(this.graph.links)
            .enter().append("line")
            .attr("stroke-width", function (d) {
                return Math.sqrt(d.value) * 1.0;
            })

        const node = svg.append("g")
            .attr("class", "nodes")
            .selectAll("g")
            .data(this.graph.nodes)
            .enter().append("g")

        node.append("title")
            .text(function (d) {
                return d.id * 2;
            });

        simulation
            .nodes(this.graph.nodes)
            .on("tick", () => {
                link
                    .attr("x1", function (d) {
                        return d.source.x;
                    })
                    .attr("y1", function (d) {
                        return d.source.y;
                    })
                    .attr("x2", function (d) {
                        return d.target.x;
                    })
                    .attr("y2", function (d) {
                        return d.target.y;
                    })

                node
                    .attr("transform", function (d) {
                        return "translate(" + d.x + "," + d.y + ")";
                    })
            });

        simulation.force("link")
            .links(this.graph.links)

        Store.state().isLoading(false)

        console.log(svg)
    }
}

const template = /* html */ `
<svg width="960" height="600"></svg>
`

export default {
    viewModel: {
        createViewModel: (...args) => new ForceGraph(args)
    },
    template
}