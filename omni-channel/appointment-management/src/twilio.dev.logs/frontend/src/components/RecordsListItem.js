import PropTypes from 'prop-types';
import * as Icon from 'react-bootstrap-icons';

const RecordsListItem = ({ data, time }) => {
  const {
    level, dateCreated, sid, message,
  } = data;
  // TODO: functionSid
  return (
    <div className="d-flex justify-content-between">
      <div
        className="px-2 border-end d-flex justify-content-center"
        style={{ paddingTop: '.8rem', width: '3rem', minWidth: '3rem' }}
      >
        {level === 'INFO'
          ? <Icon.InfoSquareFill className="text-primary h5" />
          : level === 'ERROR'
            ? <Icon.ExclamationTriangleFill className="text-danger h5" />
            : (
              <>
                <Icon.Bug />
              </>
            )}
      </div>
      <div className="flex-grow-1 px-3 py-2" style={{ wordBreak: 'break-all' }}>
        {message}
      </div>
      <div className="text-end py-2">
        {time && (
          <div className="mb-1">
            {dateCreated}
          </div>
        )}
        <div className="small text-secondary">
          {sid}
        </div>
      </div>
    </div>
  );
};

export const propTypeForItem = PropTypes.shape({
  level: PropTypes.string,
  dateCreated: PropTypes.string,
  sid: PropTypes.string,
  message: PropTypes.string,
});

RecordsListItem.propTypes = {
  data: propTypeForItem.isRequired,
  time: PropTypes.bool,
};

RecordsListItem.defaultProps = {
  time: true,
};

export default RecordsListItem;
