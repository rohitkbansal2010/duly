const nodeExternals = require('webpack-node-externals');
const IgnoreDynamicRequire = require('webpack-ignore-dynamic-require');
const CopyPlugin = require('copy-webpack-plugin');
const webpack = require('webpack');
const path = require('path');
const fs = require('fs');

// ENTRY POINTS
const FUNCTIONS_FOLDER = 'src/functions';
const entryPoints = [];
const addEntryPoint = (name) => {
  const matches = name.match(/^(.+)(\.(?:protected|private))?\.ts$/);
  matches && entryPoints.push([
    matches[1],
    path.resolve(FUNCTIONS_FOLDER, name), // `/${FUNCTIONS_FOLDER}/${name}`,
  ]);
};
fs.readdirSync(FUNCTIONS_FOLDER).forEach((name) => {
  const isDir = fs.lstatSync(path.join(FUNCTIONS_FOLDER, name)).isDirectory();
  if (!isDir) {
    addEntryPoint(name);
    return;
  }
  const dirPath = path.join(FUNCTIONS_FOLDER, name);
  fs.readdirSync(dirPath).forEach((subName) => {
    addEntryPoint(`${name}/${subName}`);
  });
});

module.exports = {
  target: 'node',
  devtool: 'source-map',
  // mode: 'production',
  mode: 'development',
  externals: [nodeExternals()],
  entry: Object.fromEntries(entryPoints),
  module: {
    rules: [{ test: /\.tsx?$/, use: 'ts-loader' }],
  },
  resolve: {
    extensions: ['.tsx', '.ts'],
  },
  output: {
    libraryTarget: 'commonjs2',
    path: path.resolve(__dirname, 'build/functions'),
    clean: true,
  },
  plugins: [
    new IgnoreDynamicRequire(),
    new CopyPlugin({
      patterns: [
        { from: 'src/assets', to: '../assets' },
      ],
    }),
    new webpack.HotModuleReplacementPlugin(),
  ],
};

//
// module.exports = {
//   // entry is where, say, your app starts - it can be called main.ts, index.ts, app.ts, whatever
//   entry: ['webpack/hot/poll?100', './src/main.ts'], // This forces webpack not to compile TypeScript for one time, but to stay running, watch for file changes in project directory and re-compile if needed
//   watch: true, // Is needed to have in compiled output imports Node.JS can understand. Quick search gives you more info
//   target: 'node', // Prevents warnings from TypeScript compiler
//   externals: [
//     nodeExternals({
//       whitelist: ['webpack/hot/poll?100'],
//     }),
//   ],
//   module: {
//     rules: [
//       {
//         test: /.tsx?$/,
//         use: 'ts-loader',
//         exclude: /node_modules/,
//       },
//     ],
//   },
//   mode: 'development',
//   resolve: {
//     extensions: ['.tsx', '.ts', '.js'],
//   },
//   plugins: [
//     new webpack.HotModuleReplacementPlugin(),
//   ],
//   output: {
//     path: path.join(__dirname, 'dist'),
//     filename: 'server.js',
//   },
// };
