'use strict'

const appInsights = require('applicationinsights');
appInsights.setup(process.env.APP_INSIGHTS_INSTRUMENTATION_KEY)
  .setAutoCollectConsole(true, true)
  .start();
appInsights.defaultClient.commonProperties = {
  ApplicationName: process.env.APPLICATION_NAME
};

const path = require('path');
const AutoLoad = require('fastify-autoload');

module.exports = async function (fastify, opts) {
  // Do not touch the following lines

  // This loads all plugins defined in plugins
  // those should be support plugins that are reused
  // through your application
  fastify.register(AutoLoad, {
    dir: path.join(__dirname, 'plugins'),
    options: Object.assign({}, opts)
  });

  // This loads all plugins defined in routes
  // define your routes in one of these
  fastify.register(AutoLoad, {
    dir: path.join(__dirname, 'routes'),
    options: Object.assign({}, opts)
  });

  fastify.ready(err => {
    if (err) throw err;
    fastify.swagger();
  });
}
