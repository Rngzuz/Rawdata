const wrapComponent = (viewModel, template, dependencies = {}) => ({
    viewModel: {
        createViewModel: (params = {}, componentInfo = {}) =>
            new viewModel({ params, componentInfo, ...dependencies })
    },
    template
})

const wrapTemplate = template => ({
    template,
    synchronous: true
})

export { wrapComponent, wrapTemplate }