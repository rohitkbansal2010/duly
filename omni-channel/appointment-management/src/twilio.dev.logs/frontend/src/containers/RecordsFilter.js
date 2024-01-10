import {
  memo, useEffect, useReducer, useRef,
} from 'react';
import PropTypes from 'prop-types';
import RecordsFilterView from '../components/RecordsFilter';
import { loadData } from '../lib/utils';
import { prepareOptions } from '../lib/filter';

const loadFilterOptions = (dispatch, type, url, params, cancelHandlers) => {
  const [promise, cancelHandler] = loadData(
    url, params, cancelHandlers[type],
  );
  cancelHandlers[type] = cancelHandler; // eslint-disable-line no-param-reassign
  promise.then(
    (options) => dispatch({ type, options: options.map(prepareOptions) }),
    ({ cancelled, message }) => cancelled || dispatch({ type, error: message }),
  );
};

const data2state = (data, currentStateData) => (data.error
  ? { error: data.error }
  : { value: data.value, options: data.options || currentStateData.options || [] });

const reducer = (state, { type, ...data }) => {
  switch (type) {
    case 'stage': {
      return {
        ...state,
        stage: data2state(data, state.stage),
        service: {},
        func: {},
        environment: {},
      };
    }
    case 'service': {
      return {
        ...state,
        service: data2state(data, state.service),
        environment: {},
        func: {},
      };
    }
    case 'environment': {
      return { ...state, environment: data2state(data, state.environment) };
    }
    case 'function': {
      return { ...state, func: data2state(data, state.func) };
    }
    default:
      return state;
  }
};

const defaultState = {
  stage: {}, service: {}, func: {}, environment: {},
};

const RecordsFilter = memo(
  ({ endpoint, onUpdate, initialState }) => {
    const [filter, dispatch] = useReducer(reducer, { ...defaultState, ...initialState });
    const {
      stage: { value: stage },
      service: { value: service },
      func: { value: func },
      environment: { value: environment },
    } = filter;
    const mounted = useRef(false);
    const cancelHandlers = useRef({ });

    // LOAD SERVICES
    useEffect(() => {
      if (mounted.current && stage) {
        loadFilterOptions(
          dispatch, 'service',
          `${endpoint}/services`, { stage },
          cancelHandlers.current,
        );
      }
    }, [endpoint, stage]);

    // LOAD ENVIRONMENTS
    useEffect(() => {
      if (mounted.current && stage && service) {
        loadFilterOptions(
          dispatch, 'environment',
          `${endpoint}/environments`, { stage, service },
          cancelHandlers.current,
        );
      }
    }, [endpoint, stage, service]);

    // LOAD FUNCTIONS
    useEffect(() => {
      if (mounted.current && stage && service) {
        loadFilterOptions(
          dispatch, 'function',
          `${endpoint}/functions`, { stage, service },
          cancelHandlers.current,
        );
      }
    }, [endpoint, stage, service]);

    // UPDATE FILTER
    useEffect(() => {
      if (mounted.current) {
        onUpdate({
          stage, service, environment, func,
        });
      }
    }, [onUpdate, stage, service, environment, func]);

    // initializing
    useEffect(() => {
      mounted.current = true;
    }, [endpoint]);

    return (
      <RecordsFilterView
        stage={filter.stage}
        service={filter.service}
        environment={filter.environment}
        func={filter.func}
        onUpdate={(type, value) => dispatch({ type, value })}
        onFetch={() => onUpdate({
          stage, service, environment, func,
        })}
      />
    );
  },
  (prev, next) => prev.endpoint === next.endpoint
        && prev.onUpdate === next.onUpdate,
);

const filterOption = PropTypes.shape({
  value: PropTypes.string,
});

RecordsFilter.propTypes = {
  endpoint: PropTypes.string.isRequired,
  onUpdate: PropTypes.func,
  initialState: PropTypes.shape({
    stage: filterOption,
    service: filterOption,
    environment: filterOption,
    func: filterOption,
  }),
};

RecordsFilter.defaultProps = {
  onUpdate: () => {},
  initialState: { },
};

export default RecordsFilter;
