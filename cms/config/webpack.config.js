const webpack = require('webpack');
const path = require('path');
const FileSystem = require("fs");
const YAML = require('yamljs');

const { WebpackManifestPlugin } = require('webpack-manifest-plugin');
const CopyWebpackPlugin = require('copy-webpack-plugin');
const { CleanWebpackPlugin } = require('clean-webpack-plugin');
const CssMinimizerPlugin = require('css-minimizer-webpack-plugin');
const MiniCssExtractPlugin = require('mini-css-extract-plugin');
const TerserJSPlugin = require('terser-webpack-plugin');

DEBUG = false;
ENV = "prod";
if (process.env.ENV) {
  ENV = process.env.ENV;
  if (ENV === "development") {
    DEBUG = true;
  }
}

HOST = '127.0.0.1';
if (process.env.HOST) {
  HOST = process.env.HOST
}

const RUNTIME_DIR = path.resolve(__dirname, '../storage/runtime');
const VIEWS_DIR = path.resolve(__dirname, '../templates');
const BUILD_DIR = path.resolve(__dirname, '../web/dist');
const SRC_DIR = path.resolve(__dirname, '../src/');
const PLUGINS_DIR = path.resolve(__dirname, '../plugins/');
const NODE_MODULES = path.resolve(__dirname, '../node_modules');
const ROOT_DIR = path.resolve(__dirname, '../');

console.log("HOST", HOST);
console.log('NODE_ENV', process.env.NODE_ENV);
console.log('BUILD_FOR', ENV);
console.log('NODE_MODULES', NODE_MODULES);
console.log('RUNTIME_DIR', RUNTIME_DIR);
console.log('VIEWS_DIR', VIEWS_DIR);
console.log('BUILD_DIR', BUILD_DIR);
console.log('SRC_DIR', SRC_DIR);
console.log('ROOT_DIR', ROOT_DIR);

const pageEntries = () => {
  const pages = FileSystem.readdirSync(path.resolve(__dirname, '../src/pages'))
  const entries = {}
  pages.map((page) => {
    entries[page] = path.resolve(__dirname, `../src/pages/${page}/index.js`)
  })
  return entries
}

module.exports = env => {
  // grab NODE_ENV from process
  env = Object.assign({},
    env === null ? {} : env,
    { 'NODE_ENV': process.env.NODE_ENV }
  );

  return {
    mode: env.NODE_ENV,
    entry: {
      main: ['core-js', SRC_DIR + '/js/polyfills.js', SRC_DIR + '/js/main.js'],
      mapbox: [SRC_DIR + '/js/mapbox.js'],
      eventRegistrationsPlugin: ['core-js', SRC_DIR + '/js/polyfills.js', PLUGINS_DIR + '/event-registration/src/assetbundles/EventRegistrationPlugin/js/EventRegistration.js'],
      ...pageEntries()
    },
    resolve: {
      alias: {
        normalize: path.join(__dirname, '/node_modules/normalize.css/normalize.css')
      }
    },
    optimization: {
      removeAvailableModules: env.NODE_ENV == 'production',
      removeEmptyChunks: env.NODE_ENV == 'production',
      moduleIds: 'deterministic',
      splitChunks: env.NODE_ENV == 'production' ? {
        chunks: 'async',
        minSize: 20000,
        minRemainingSize: 0,
        minChunks: 1,
        maxAsyncRequests: 30,
        maxInitialRequests: 30,
        enforceSizeThreshold: 50000,
        cacheGroups: {
          defaultVendors: {
            test: /[\\/]node_modules[\\/]/,
            priority: -10,
            reuseExistingChunk: true,
          },
          default: {
            minChunks: 2,
            priority: -20,
            reuseExistingChunk: true,
          },
        },
      } : false,
      usedExports: true,
      minimizer: env.NODE_ENV == 'production' ? [
        new TerserJSPlugin({
          test: /\.js(\?.*)?$/i,
          parallel: true,
          extractComments: true,
          terserOptions: {
            ecma: 5,
            warnings: false,
            mangle: true,
            safari10: true,
            compress: {
              drop_console: true
            }
          },
        }),
        new CssMinimizerPlugin({
          minify: CssMinimizerPlugin.cssnanoMinify,
          minimizerOptions: {
            present: [
              'default',
              { discardComments: { removeAll: true }, reduceIdents: false }
            ]
          }
        })
      ] : [],
    },
    devtool: env.NODE_ENV == 'production' ? 'source-map' : 'eval-cheap-module-source-map',
    output: {
      path: BUILD_DIR,
      filename: '[name].[fullhash].bundle.js',
      publicPath: '/dist/'
    },
    devServer: {
      public: 'https://' + HOST,
      contentBase: BUILD_DIR,
      publicPath: '/dist/',
      port: 8080,
      sockPort: 'location',
      host: '0.0.0.0',
      compress: true,
      hot: true,
      open: false,
      stats: 'normal',
      mimeTypes: { "application/x-protobuf": ["pbf"] },
      overlay: {
        errors: true,
        warnings: true,
      },
      writeToDisk: false,
      clientLogLevel: 'debug',
      disableHostCheck: true,
      watchOptions: {
        aggregateTimeout: 300
      }
    },
    module: {
      rules: [
        {
          test: /\.(js|jsx)$/,
          exclude: /node_modules/,
          include: SRC_DIR,
          use: {
            loader: 'babel-loader',
            options: {
              cacheDirectory: true
            }
          }
        },
        // Bypass automatic `.json` file processing for webpack
        {
          type: 'javascript/auto',
          test: /\.json$/,
          loader: 'file-loader',
          options: {
            name: '[name].[ext]',
          }
        },
        {
          test: /\.yaml$/,
          loader: 'yaml',
        },
        {
          test: /\.html$/,
          loader: 'html-loader'
        },
        {
          test: /\.(sa|sc|c)ss$/,
          use: [
            {
              loader: MiniCssExtractPlugin.loader
            },
            {
              loader: 'css-loader',
              options: {
                sourceMap: true
              }
            },
            'resolve-url-loader',
            {
              loader: 'sass-loader',
              options: {
                sassOptions: {
                  includePaths: ['./node_modules']
                },
                // Temporarily disabled; fails to compile in a Drone build
                // // the prependData option is used to avoid having to manually import the global `_variables.scss` and `_mixins.scss` files into every file under `/src/scss/pages`. 
                // prependData: (node) => node._module.context === '/var/www/src/scss/pages'
                //   ? `@import "../variables";\n@import "../mixins";\n`
                //   : '/* duly */',
                // sourceMap: true
              }
            },
            {
              loader: '@epegzz/sass-vars-loader',
              options: {
                vars: {
                  DEBUG: DEBUG,
                  ENV: ENV
                }
              }
            }
          ],
        },
        {
          test: /\.twig$/,
          use: [{
            loader: "file-loader"
          }]
        },
        {
          test: /\.(png|jpg|jpeg|gif|ico)$/,
          use: [
            {
              // loader: 'url-loader'
              loader: 'file-loader',
              options: {
                name: './img/[name].[fullhash].[ext]'
              }
            }
          ]
        },
        {
          test: /\.(woff(2)?|ttf|eot|svg)(\?v=\d+\.\d+\.\d+)?$/,
          use: [{
            loader: 'file-loader',
            options: {
              name: './fonts/[name].[ext]',
            }
          }]
        }]
    },
    plugins: [
      new WebpackManifestPlugin(),
      new webpack.DefinePlugin({
        ENV: ENV
      }),
      new webpack.IgnorePlugin({
        resourceRegExp: /^\.\/locale$/,
        contextRegExp: /moment$/,
      }),      
      new webpack.HotModuleReplacementPlugin(),
      new CopyWebpackPlugin({
        patterns: [
          {
            context: SRC_DIR + '/img/',
            from: '**/*',
            to: BUILD_DIR + '/img/',
            force: true
          },
          {
            context: NODE_MODULES + '/klokantech-gl-fonts/',
            from: '**/*',
            to: BUILD_DIR + '/fonts/pbf',
            force: true
          }
        ]
      }),
      new MiniCssExtractPlugin({
        // Options similar to the same options in webpackOptions.output
        // both options are optional
        filename: '[name].[fullhash].styles.css',
        chunkFilename: '[id].[fullhash].css',
      }),
      new CleanWebpackPlugin({
        root: BUILD_DIR,
        verbose: true,
        dry: false
      })
    ]
  }
};