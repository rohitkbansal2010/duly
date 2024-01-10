import PropTypes from 'prop-types';
import RecordsListItem, { propTypeForItem } from './RecordsListItem';

const RecordsList = ({ data, next, previous }) => {
  if (data && data.length) {
    return (
      <>
        <ul className="list-group my-5">
          {data.map((item) => {
            const { executionId } = item;
            if (executionId) {
              return (
                <li key={item.executionId} className="list-group-item ps-1">
                  <div className="d-flex">
                    <div style={{ width: '3rem' }} className="border-end">
                        &nbsp;
                    </div>
                    <div className="ps-3 pb-2 text-secondary flex-grow-1">
                      {executionId}
                    </div>
                    <div>
                      {item.dateCreated}
                    </div>
                  </div>
                  {item.list.map((executionItem) => (
                    <div key={executionItem.sid} className="">
                      <RecordsListItem data={executionItem} time={false} />
                    </div>
                  ))}
                </li>
              );
            }
            return (
              <li key={item.sid} className="list-group-item ps-1">
                <RecordsListItem data={item} />
              </li>
            );
          })}
        </ul>
        <div className="d-flex justify-content-center pb-5">
          <button type="button" disabled={!previous} className="btn btn-secondary mx-3" onClick={previous}>Previous</button>
          <button type="button" disabled={!next} className="btn btn-secondary mx-3" onClick={next}>Next</button>
        </div>
      </>
    );
  }
  return (
    <div className="text-secondary my-5">There is no data</div>
  );
};

RecordsList.propTypes = {
  data: PropTypes.arrayOf(propTypeForItem),
  next: PropTypes.func,
  previous: PropTypes.func,
};

RecordsList.defaultProps = {
  data: undefined,
  next: undefined,
  previous: undefined,
};

export default RecordsList;
