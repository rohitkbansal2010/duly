const CopyWebpackPlugin = require('copy-webpack-plugin');
const Dotenv = require('dotenv-webpack');
const { GitRevisionPlugin } = require('git-revision-webpack-plugin');
const HtmlWebpackPlugin = require('html-webpack-plugin');
const threadLoader = require('thread-loader');
const webpack = require('webpack');

const path = require('path');

const gitRevisionPlugin = new GitRevisionPlugin();

const threadLoaderOptions = {
  workers: 6,
  workerParallelJobs: 60,
  poolRespawn: false,
  poolParallelJobs: 60,
};

threadLoader.warmup(threadLoaderOptions, [ 'babel-loader', 'sass-loader', 'css-loader' ]);

module.exports = {
  entry: path.resolve(__dirname, '..', './src/index.tsx'),
  resolve: {
    alias: {
      '@components': path.resolve(__dirname, '..', './src/components'),
      '@mock-data': path.resolve(__dirname, '..', './src/mock-data'),
      '@enums': path.resolve(__dirname, '..', './src/common/enums'),
      '@hooks': path.resolve(__dirname, '..', './src/common/hooks'),
      '@localization': path.resolve(__dirname, '..', './src/localization'),
      '@pages': path.resolve(__dirname, '..', './src/pages'),
      '@redux': path.resolve(__dirname, '..', './src/redux'),
      '@routes': path.resolve(__dirname, '..', './src/routes'),
      '@utils': path.resolve(__dirname, '..', './src/common/utils'),
      '@constants': path.resolve(__dirname, '..', './src/common/constants'),
      '@icons': path.resolve(__dirname, '..', './src/common/icons'),
      '@images': path.resolve(__dirname, '..', './src/common/images'),
      '@styles': path.resolve(__dirname, '..', './src/common/styles'),
      '@types': path.resolve(__dirname, '..', './src/common/types'),
      '@ui-kit': path.resolve(__dirname, '..', './src/ui-kit'),
      '@interfaces': path.resolve(__dirname, '..', './src/common/interfaces'),
      '@fonts': path.resolve(__dirname, '..', './src/fonts'),
      '@sagas': path.resolve(__dirname, '..', './src/sagas'),
      '@http': path.resolve(__dirname, '..', './src/http'),
    },
    extensions: [ '.ts', '.tsx', '.js' ],
  },
  module: {
    rules: [
      {
        test: /\.ts(x)?$/,
        exclude: /node_modules/,
        use: [
          {
            loader: 'thread-loader',
            options: threadLoaderOptions,
          },
          { loader: 'babel-loader' },
        ],
      },
      {
        test: /\.css$/,
        use: [ 'style-loader', 'css-loader' ],
      },
      {
        test: /\.scss$/,
        use: [
          { loader: 'style-loader' },
          {
            loader: 'thread-loader',
            options: threadLoaderOptions,
          },
          {
            loader: 'css-loader',
            options: {
              modules: { localIdentName: '[local]--[hash:base64:8]' },
              sourceMap: true,
            },
          },
          'resolve-url-loader',
          {
            loader: 'thread-loader',
            options: threadLoaderOptions,
          },
          { loader: 'sass-loader' },
        ],
      },
      {
        test: /\.(?:ico|gif|png|jpg|jpeg)$/i,
        type: 'asset/resource',
      },
      {
        test: /\.(woff(2)?|eot|ttf|otf|svg)$/,
        type: 'asset/inline',
      },
    ],
  },
  output: {
    publicPath: '/',
    path: path.resolve(__dirname, '..', './dist'),
    filename: '[name].[contenthash].js',
  },
  optimization: {
    runtimeChunk: 'single',
    splitChunks: {
      cacheGroups: {
        vendor: {
          test: /[\\/]node_modules[\\/]/,
          name: 'vendors',
          chunks: 'all',
        },
      },
    },
  },
  cache: true,
  plugins: [
    new HtmlWebpackPlugin({ template: path.resolve(__dirname, '..', './src/index.html') }),
    new CopyWebpackPlugin({ patterns: [ { from: './src/assets' } ] }),
    new Dotenv(),
    new webpack.DefinePlugin({
      'process.env.BUILD_NUMBER': JSON.stringify(process.env.BUILD_BUILDNUMBER),
      'process.env.GIT_VERSION': JSON.stringify(gitRevisionPlugin.version()),
    }),
  ],
};
