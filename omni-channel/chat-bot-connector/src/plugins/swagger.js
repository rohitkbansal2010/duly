'use strict'

const fp = require('fastify-plugin')

/**
 * A Fastify plugin for serving a Swagger UI
 *
 * @see https://github.com/fastify/fastify-swagger
 */
module.exports = fp(async function (fastify, opts) {
  fastify.register(require('fastify-swagger'), {
    routePrefix: 'documentation',
    exposeRoute: true,
    swagger: {
      info: {
        title: 'Chatbot Epic Connector',
        description: 'Epic connector for Freshworks chat bot',
        version: '0.0.1',
      }
    },
  });
});
