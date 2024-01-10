import axios from 'axios';

// eslint-disable-next-line import/prefer-default-export
export function loadData(url, params, cancelHandler) {
  if (cancelHandler) {
    cancelHandler();
  }
  let nextCancelHandler = null;
  const promise = axios({
    method: 'get',
    url,
    params,
    cancelToken: new axios.CancelToken((cancel) => { nextCancelHandler = cancel; }),
  }).then(
    ({ data }) => data,
    (error) => Promise.reject(axios.isCancel(error) ? { cancelled: true } : error),
  );
  return [promise, nextCancelHandler];
}
