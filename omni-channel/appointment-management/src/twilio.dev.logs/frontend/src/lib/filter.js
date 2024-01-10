import { loadData } from './utils';

export const prepareOptions = ({ sid, friendlyName, uniqueName }) => ({
  name: friendlyName || uniqueName,
  value: sid,
});

export function getFilterValuesFromState(filterState) {
  const {
    stage = {}, service = {}, func = {}, environment = {},
  } = filterState;
  return {
    stage: stage.value, service: service.value, func: func.value, environment: environment.value,
  };
}

const loadOptions = async (url, params, value) => {
  try {
    const options = await loadData(url, params)[0];
    return { value, options: options.map(prepareOptions) };
  } catch (error) {
    return { value: undefined, error: error.message };
  }
};

export async function loadFilterInitialState(endpoint, filter = {}) {
  try {
    const stage = await loadOptions(`${endpoint}/stages`, undefined, filter.stage);

    const service = stage.value
      ? await loadOptions(`${endpoint}/services`, { stage: stage.value }, filter.service)
      : { value: filter.service };

    const environment = service.value
      ? await loadOptions(`${endpoint}/environments`, { stage: stage.value, service: service.value }, filter.environment)
      : { value: filter.environment };

    const func = service.value
      ? await loadOptions(`${endpoint}/functions`, { stage: stage.value, service: service.value }, filter.func)
      : { value: filter.func };

    return {
      stage, service, environment, func,
    };
  } catch (error) {
    return { error: error.message };
  }
}
