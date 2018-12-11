const path = require('path')
const HtmlWebPackPlugin = require('html-webpack-plugin')

module.exports = {
    entry: './src/main.js',
    devServer: { historyApiFallback: true },
    output: {
        filename: '[name].bundle.js',
        path: path.resolve(__dirname, './dist')
    },
    optimization: {
        splitChunks: {
            chunks: 'all'
        }
    },
    resolve: {
        alias: {
            '@': path.resolve(__dirname, './src'),
            'Bindings': path.resolve(__dirname, './src/bindings'),
            'Components': path.resolve(__dirname, './src/components'),
            'Services': path.resolve(__dirname, './src/services')
        }
    },
    module: {
        rules: [
            {
                test: /\.html$/,
                use: [{
                    loader: 'html-loader',
                    options: { minimize: true }
                }]
            },
            {
                test: /\.css$/,
                use: [
                    'style-loader',
                    'css-loader'
                ]
            }
        ]
    },
    plugins: [
        new HtmlWebPackPlugin({
            template: './public/index.html',
            filename: './index.html'
        })
    ]
}