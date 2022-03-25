const path = require('path');

module.exports = {
    mode: 'development',
    entry: './ts/index.ts',
    module: {
        rules: [
            {
                test: /\.ts$/,
                use: 'ts-loader',
                exclude: /node_modules/,
            },
        ],
    },
    resolve: {
        extensions: [".tsx", ".ts", ".js"]
    },
    output: {
        path: path.resolve(__dirname, 'wwwroot', 'dist'),
        filename: 'bundle.js'
    }
}
