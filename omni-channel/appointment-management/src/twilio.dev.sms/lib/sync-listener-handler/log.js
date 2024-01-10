exports.handler = async (context, event, callback) => {
  console.log('log', event);

  const client = context.getTwilioClient();
  await client.sync.services(context.SYNC_SERVICE_SID)
    .syncLists(context.SYNC_LOG_LIST_NAME)
    .syncListItems
    .create({ data: event, ttl: context.SYNC_LIST_ITEM_TTL });

  return callback(null);
};
