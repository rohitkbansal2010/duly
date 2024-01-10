import { memo } from 'react';
import PropTypes from 'prop-types';

const styles = {
  loader: {
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
  },
};

const Loader = memo(({ active, text }) => {
  if (active) {
    return (
      <div style={styles.loader}>
        <div className="spinner-border" role="status" style={{ width: '4rem', height: '4rem' }}>
          <span className="visually-hidden">Loading...</span>
        </div>
        {text && (<div className="my-4">{text}</div>)}
      </div>
    );
  }
  return <></>;
});

Loader.propTypes = {
  active: PropTypes.bool,
  text: PropTypes.string,
};

Loader.defaultProps = {
  active: true,
  text: undefined,
};

export default Loader;
