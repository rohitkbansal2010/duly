import PropTypes from 'prop-types';
import Loader from './Loader';

const styles = {
  background: 'rgba(255, 255, 255, 0.7)',
  position: 'absolute',
  top: 0,
  left: 0,
  width: '100%',
  height: '100%',
  zIndex: 100,
  display: 'flex',
  flexDirection: 'column',
  justifyContent: 'center',
  alignItems: 'center',
};

const ConfigurationScreen = ({ error }) => {
  if (error) {
    return (
      <div style={styles}>
        <div className="text-danger h5">{error}</div>
        <a className="small text-secondary" href="/">Try to reload the page</a>
      </div>
    );
  }
  return (
    <Loader text="configuration..." />
  );
};

ConfigurationScreen.propTypes = {
  error: PropTypes.string,
};

ConfigurationScreen.defaultProps = {
  error: undefined,
};

export default ConfigurationScreen;
