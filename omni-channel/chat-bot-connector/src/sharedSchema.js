const sharedSchema = {
  response: {
    404: {
      description: 'None found',
      type: 'object',
      properties: {
        status: {
          type: 'number',
        },
        body: {
          type: 'object',
          properties: {
            success: { type: 'boolean' },
            message: { type: 'string' },
          }
        }
      }
    },
    500: {
      description: 'Internal server error',
      type: 'object',
      properties: {
        status: { type: 'number' },
        body: {
          type: 'object',
          properties: {
            success: { type: 'boolean' },
            message: { type: 'string' },
          }
        }
      }
    }
  }
};

module.exports = { sharedSchema };