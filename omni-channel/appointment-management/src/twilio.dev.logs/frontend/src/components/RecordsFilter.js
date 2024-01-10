import PropTypes from 'prop-types';
import RecordsFilterSelect from './RecordsFilterSelect';

const RecordsFilter = ({
  stage, service, environment, func, onUpdate, onFetch,
}) => (
  <form
    className="row"
    onSubmit={(event) => {
      event.preventDefault();
      onFetch();
    }}
  >
    <div className="col-2">
      <RecordsFilterSelect
        title="account:"
        options={stage.options}
        value={stage.value}
        error={stage.error}
        onChange={(value) => onUpdate('stage', value)}
      />
    </div>
    <div className="col-3">
      <RecordsFilterSelect title="service:" options={service.options} value={service.value} error={service.error} onChange={(value) => onUpdate('service', value)} />
    </div>
    <div className="col-2">
      <RecordsFilterSelect title="env:" options={environment.options} value={environment.value} error={environment.error} onChange={(value) => onUpdate('environment', value)} />
    </div>
    <div className="col-4">
      <RecordsFilterSelect title="function:" options={func.options} value={func.value} error={func.error} onChange={(value) => onUpdate('function', value)} />
    </div>
    <div className="col-1">
      <button type="submit" className="btn btn-primary mb-2">Fetch</button>
    </div>
  </form>
);

const filterOption = PropTypes.shape({
  value: PropTypes.string,
  options: PropTypes.arrayOf(
    PropTypes.shape({
      name: PropTypes.string.isRequired,
      value: PropTypes.string.isRequired,
    }),
  ),
  error: PropTypes.string,
});

RecordsFilter.propTypes = {
  stage: filterOption,
  service: filterOption,
  environment: filterOption,
  func: filterOption,
  onUpdate: PropTypes.func,
  onFetch: PropTypes.func,
};

RecordsFilter.defaultProps = {
  stage: {},
  service: {},
  environment: {},
  func: {},
  onUpdate: () => {},
  onFetch: () => {},
};

export default RecordsFilter;
