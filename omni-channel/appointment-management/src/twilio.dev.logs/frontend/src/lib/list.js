import { loadData } from './utils';

const EXECUTION_ID_REGEX = /^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}/i;

export function prepareListData({ nextPageUrl, previousPageUrl, instances }) {
  const list = [];
  const map = new Map();
  [...instances].reverse().forEach((item) => {
    const { message } = item;
    const [executionId] = EXECUTION_ID_REGEX.exec(message) || [];
    if (!executionId) {
      list.push(item);
      return;
    }
    if (map.has(executionId)) {
      map.get(executionId).list.push(
        { ...item, message: message.substring(executionId.length + 1) },
      );
    } else {
      const execution = {
        executionId,
        list: [{ ...item, message: message.substring(executionId.length + 1) }],
        dateCreated: item.dateCreated,
      };
      map.set(executionId, execution);
      list.push(execution);
    }
  });
  return {
    list: list.reverse(),
    next: nextPageUrl,
    previous: previousPageUrl,
  };
}

export async function loadListInitialState(endpoint, filter) {
  if (filter.stage && filter.service && filter.environment) {
    try {
      return await loadData(`${endpoint}/logs`, filter)[0];
    } catch ({ message, cancelled }) {
      return cancelled ? { } : { error: message };
    }
  } else {
    return {};
  }
}
