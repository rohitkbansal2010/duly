import { useCallback } from 'react';
import { Row, Col } from 'react-bootstrap';
import { useDispatch, useSelector } from 'react-redux';

import { SitesContainer } from '@components/sites-container';
import { MONTH_NAMES_SHORT, WEEKDAYS_NAMES } from '@constants';
import { mapPinMagenta } from '@icons';
import { showLocationPickerModal } from '@redux/actions';
import { RootState } from '@redux/reducers';
import { formatAddressBottomLine } from '@utils';

import styles from './appointment-daily-statistics.scss';
import { NewPatients } from './new-patients';

export const AppointmentDailyStatistics = () => {
  const dispatch = useDispatch();
  const today = new Date();

  const dailyStatistics = useSelector(({ TODAYS_APPOINTMENTS }: RootState) =>
    TODAYS_APPOINTMENTS.dailyStatistics);
  const currentSite = useSelector(({ SITE }: RootState) =>
    SITE.currentSite);

  const addressBottomLine = currentSite?.address ? formatAddressBottomLine(currentSite.address) : '';

  const handleClick = useCallback(() => {
    dispatch(showLocationPickerModal());
  }, [ dispatch ]);

  const newPatients = dailyStatistics && dailyStatistics.newPatients ?
    <NewPatients newPatients={dailyStatistics.newPatients} /> : null;

  return (
    <Col>
      <div className={styles.topSection}>
        <Row className={styles.dateRow}>
          <span className={styles.dateRowDay} data-testid="dateRowDay">{WEEKDAYS_NAMES[today.getDay()]},</span>
          <span className={styles.dateRowMonth} data-testid="dateRowMonth">
            {today.getDate()} {MONTH_NAMES_SHORT[today.getMonth()]}
          </span>
          <span className={styles.dateRowYear} data-testid="dateRowYear">{today.getFullYear()}</span>
        </Row>
        <Row onClick={handleClick} className={styles.addressRow} data-testid="addressRow">
          {currentSite && (
            <SitesContainer>
              <span className={styles.addressRowTopPart} data-testid="addressRowTopPart">
                <img src={mapPinMagenta} alt="map pin magenta" />
                <span className={styles.addressRowTopPartSpacingSpan} data-testid="addressRowTopPartSpacingSpan">
                  {currentSite?.address?.line}
                </span>
              </span>
              <span className={styles.addressRowBottomPart} data-testid="addressRowBottomPart">
                {addressBottomLine}
              </span>
            </SitesContainer>
          )}
        </Row>
      </div>
      <div className={styles.bottomSection}>
        {newPatients}
      </div>
    </Col>
  );
};
