import { forwardRef } from 'react';
import PropTypes from 'prop-types';

const RecordsFilterSelect = forwardRef(({
  title, value, options, onChange, error,
}, ref) => {
  const loading = options === null;
  return (
    <div className="input-group mb-3">
      <div className="input-group-prepend">
        <span className="input-group-text">{title}</span>
      </div>
      {error ? (
        <div className="form-control text-danger">{error}</div>
      ) : (
        <select
          value={value || 0}
          disabled={loading}
          className="form-control"
          ref={ref}
          onChange={onChange && ((event) => onChange(event.target.value === '0' ? undefined : event.target.value))}
        >
          <option value={0}>...</option>
          {options && options.map(({ name, value: optionValue }) => (
            <option value={optionValue} key={optionValue}>{name}</option>
          ))}
        </select>
      )}
    </div>
  );
});

RecordsFilterSelect.propTypes = {
  title: PropTypes.string.isRequired,
  options: PropTypes.arrayOf(PropTypes.object),
  value: PropTypes.string,
  onChange: PropTypes.func,
  error: PropTypes.string,
};

RecordsFilterSelect.defaultProps = {
  options: [],
  value: undefined,
  onChange: () => {},
  error: undefined,
};

export default RecordsFilterSelect;
