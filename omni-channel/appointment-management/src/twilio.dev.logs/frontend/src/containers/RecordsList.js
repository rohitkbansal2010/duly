import { useEffect, useReducer, useRef } from 'react';
import PropTypes from 'prop-types';
import { loadData } from '../lib/utils';
import { prepareListData } from '../lib/list';
import Loader from '../components/Loader';
import RecordsListView from '../components/RecordsList';

const loadPage = (ctx, dispatch, endpoint, params) => {
  dispatch({ type: 'loading' });
  const [promise, cancelHandler] = loadData(endpoint, params, ctx.dataCancelHandler);
  ctx.dataCancelHandler = cancelHandler;
  promise.then(
    (result) => dispatch({ type: 'data', ...result }),
    ({ message, cancelled }) => cancelled || dispatch({ type: 'error', message }),
  );
};

const reducer = (state, { type, ...data }) => {
  switch (type) {
    case 'loading':
      return { ...state, loading: true };
    case 'error':
      return { loading: false, error: data.message };
    case 'data': {
      return prepareListData(data);
    }
    default:
      return state;
  }
};

const prepareInitialState = (data) => {
  if (!data) {
    return { loading: true, data: {} };
  }
  const { error, instances } = data;
  if (error) {
    return { error };
  }
  if (instances) {
    return prepareListData(data);
  }
  return { error: 'Please, update the filter' };
};

export default function RecordsList({ endpoint, filter, initialState }) {
  const ctx = useRef({ mounted: false, dataCancelHandler: undefined });
  const [state, dispatch] = useReducer(reducer, initialState, prepareInitialState);

  // DATA FETCHING
  useEffect(() => {
    if (!ctx.current.mounted) {
      return;
    }
    if (filter.stage && filter.service && filter.environment) {
      loadPage(ctx.current, dispatch, `${endpoint}/logs`, filter);
    } else {
      dispatch({ type: 'error', message: 'Stage, Service and Environment are required' });
      if (ctx.current.dataCancelHandler) {
        ctx.current.dataCancelHandler();
        ctx.current.dataCancelHandler = undefined;
      }
    }
  }, [endpoint, filter]);

  // MOUNTING
  useEffect(() => {
    ctx.current.mounted = true;
  }, []);

  const {
    loading, error, list, next, previous,
  } = state;

  return (
    <>
      <Loader active={!!loading} />
      {error ? (
        <div className="alert alert-danger my-5">{error}</div>
      ) : (
        <RecordsListView
          data={list}
          next={next ? () => {
            loadPage(ctx.current, dispatch, `${endpoint}/logs-page`, { ...filter, link: next });
          } : undefined}
          previous={previous ? () => {
            loadPage(ctx.current, dispatch, `${endpoint}/logs-page`, { ...filter, link: previous });
          } : undefined}
        />
      )}
    </>
  );
}

RecordsList.propTypes = {
  initialState: PropTypes.any, // eslint-disable-line react/forbid-prop-types
  endpoint: PropTypes.string.isRequired,
  filter: PropTypes.shape({
    stage: PropTypes.string,
    service: PropTypes.string,
    environment: PropTypes.string,
    func: PropTypes.string,
  }),
};

RecordsList.defaultProps = {
  initialState: undefined,
  filter: PropTypes.shape({
    stage: undefined,
    service: undefined,
    func: undefined,
  }),
};
