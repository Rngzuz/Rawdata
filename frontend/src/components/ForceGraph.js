import * as d3 from 'd3'
import Store from '@/Store.js'
import { Component } from './Component.js'
import { pureComputed } from 'knockout'

import SearchService from "@/services/SearchService";

class ForceGraph extends Component {
    constructor(args) {
        super(args)
        this.isLoading = pureComputed(
            () => Store.state().isLoading()
        )

        Store.state().search.subscribe(v => {
            d3.select(this.$el.querySelector('svg')).selectAll("*").remove()
            Store.state().isLoading(true)
            this.fetchGraphInput(v[0])
        })
    }

    async fetchGraphInput(word) {
        Store.state().isLoading(true)

        let result = await SearchService.getForceGraphInput(word)
        this.init(result)
    }


    init(graphData) {

        const svg = d3
            .select(this.$el.querySelector('svg'))

        const width = +svg.attr("width")
        const height = +svg.attr("height")

        const color = d3
            .scaleOrdinal(d3.schemeCategory10)

        const simulation = d3
            .forceSimulation()
            .force("link", d3.forceLink().id(d => d.id).strength(0.001))
            .force("charge", d3.forceManyBody())
            .force("center", d3.forceCenter(width / 2, height / 2))

        const link = svg
            .append("g")
            .attr("class", "links")
            .selectAll("line")
            .data(graphData.links)
            .enter()
            .append("line")
            .attr("stroke-width", d => Math.sqrt(d.value) * 1.0)

        const node = svg
            .append("g")
            .attr("class", "nodes")
            .selectAll("g")
            .data(graphData.nodes)
            .enter()
            .append("g")

        node
            .append("circle")
            .attr("r", 8)
            .attr("fill", d => color(d.group))
            .call(
                d3.drag()
                .on("start", d => this.dragstarted(d, simulation))
                .on("drag",  d => this.dragged(d, simulation))
                .on("end", d => this.dragended(d, simulation))
            )

        node
            .append("text")
            .text(d => d.id)
            .style("font-size", "14px")
            .attr('x', 8)
            .attr('y', 3)

        node
            .append("title")
            .text(d => d.id * 2)

        simulation
            .nodes(graphData.nodes)
            .on("tick", () => {
                link
                    .attr("x1", d => d.source.x)
                    .attr("y1", d => d.source.y)
                    .attr("x2", d => d.target.x)
                    .attr("y2", d => d.target.y)

                node.attr("transform", d => "translate(" + d.x + "," + d.y + ")")
            })

        simulation
            .force("link")
            .links(graphData.links)


        Store.state().isLoading(false)
    }


    dragstarted(d, simulation) {
        if (!d3.event.active) simulation.alphaTarget(0.3).restart();
        d.fx = d.x;
        d.fy = d.y;
    }

    dragged(d, simulation) {
        d.fx = d3.event.x;
        d.fy = d3.event.y;
    }

    dragended(d, simulation) {
        if (!d3.event.active) simulation.alphaTarget(0);
        d.fx = null;
        d.fy = null;
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