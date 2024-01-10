import { useEffect, useRef, useState } from 'react';
import RecordsFilter from './containers/RecordsFilter';
import RecordsList from './containers/RecordsList';
import ConfigurationScreen from './components/ConfigurationScreen';
import { getFilterValuesFromState, loadFilterInitialState } from './lib/filter';
import { loadListInitialState } from './lib/list';

const ENDPOINT = process.env.REACT_APP_BACKEND_URL || '';

function saveFilterValues(values) {
  localStorage.setItem('filterValues', JSON.stringify(values));
}

function getFilterValues() {
  try {
    const values = localStorage.getItem('filterValues');
    return values ? JSON.parse(values) : undefined;
  } catch (error) {
    // do nothing
  }
  return undefined;
}

function App() {
  const [ready, setReady] = useState({ loading: true });
  const [filter, setFilter] = useState({});
  const ctx = useRef({ mounted: false, dataCancelHandler: undefined });

  // SAVE FILTER TO LOCALE STORAGE
  useEffect(() => {
    ctx.current.mounted && saveFilterValues(filter);
  }, [filter]);

  // INITIALIZATION
  useEffect(() => {
    ctx.current.mounted = true;
    (async () => {
      const initialFilter = await loadFilterInitialState(ENDPOINT, getFilterValues());
      const initialList = await loadListInitialState(
        ENDPOINT,
        getFilterValuesFromState(initialFilter),
      );
      setReady({ filter: initialFilter, list: initialList });
    })();
  }, []);

  if (ready.loading) {
    return (
      <ConfigurationScreen error={ready.error} />
    );
  }

  return (
    <>
      <div className="container mt-3">
        <RecordsFilter
          initialState={ready && ready.filter}
          endpoint={ENDPOINT}
          onUpdate={setFilter}
        />
      </div>
      <hr />
      <div className="container" style={{ position: 'relative', minHeight: '5rem' }}>
        <RecordsList
          initialState={ready && ready.list}
          endpoint={ENDPOINT}
          filter={filter}
        />
      </div>
    </>
  );
}

export default App;
