module.exports = {
  mode: 'development',
  devServer: {
    hot: true,
    open: true,
    historyApiFallback: true,
    static: './config',
  },
  devtool: 'cheap-module-source-map',
};
