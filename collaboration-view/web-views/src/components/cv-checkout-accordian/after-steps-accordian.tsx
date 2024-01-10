import { Accordion } from 'react-bootstrap';
import { useSelector } from 'react-redux';

import { HeaderDataType } from '@mock-data';
import { RootState } from '@redux/reducers';

import { Card } from '../after-visit-steps-complete/card';

import styles from './accordian.scss';

type CardColumnsType = {
  topLine: string,
  middleLine?: string,
  bottomLine?: string,
}
type CardHeaderType = {
  dateAndTime?: string,
  providerDetails?: {
    name: string,
    photo?: string,
  },
}
export type CardDetailsType = {
  date?: string,
  cardHeader?: CardHeaderType[],
  cardColumns: CardColumnsType[],
};
export type AfterStepsAccordianType = {
  header: HeaderDataType,
  cardDetails: CardDetailsType[],
  cardClass?: string,
}
export const AfterStepsAccordian = ({ 
  header, 
  cardDetails, 
  cardClass,
}: AfterStepsAccordianType) => {
  const { isLabTestScheduled } = useSelector(({ CHECKOUT_APPOINTMENTS }: RootState) =>
    CHECKOUT_APPOINTMENTS);

  return (
    <>
      <style type="text/css">
        {`
              .accordion-button:focus{
                box-shadow: none;
              }
              .accordian, .accordian*{
                border:none !important;
              }
              .accordian{
                padding: 1rem !important;
              }
              .accordion-body{
                padding: 0rem;
              }
              .accordion-item{
                border-radius: 0.625rem !important;
                border: none !important;
              }
              .accordion-button:not(.collapsed){
                color:rgba(15, 23, 42, 1) !important;
                background-color: white;
              }
              .container{
                max-width:100%;
              }
              .row > * {
                padding: 0;
              }
              .accordion-button:not(.collapsed){
                box-shadow: none;
              }
            `}
      </style>
      <div className={styles.topDiv}>
        <div className={styles.accordianOuterDiv}>
          <Accordion defaultActiveKey="0">
            <Accordion.Item eventKey="0" data-automation={`after-steps-accordion-${header.heading.toLowerCase().replaceAll(' ', '-')}`}>
              <Accordion.Header>
                <div className={styles.accordianHeader}>
                  {header.heading === 'Follow-Up Appointment' ?
                    <img src={header.icon} alt="doctor" className={styles.accordianHeaderFollowUpImage} />
                  : <img src={header.icon} alt="doctor" className={styles.accordianHeaderImage} />
                }
                  <div>
                    <div className={styles.headerData}>
                      <div className={styles.pageHeading}>{header.heading}
                        {cardDetails && cardDetails.length > 1 ? (
                          <span className={styles.scheduleLength}>
                            ( {cardDetails.length} ) </span>
                      ) : ''}
                      </div>
                      {cardDetails && cardDetails.length === 1 ? (
                        <div className={styles.date}>
                          {cardDetails[0].date}
                        </div>
                    ) : ''}
                    </div>
                  </div>
                </div>
              </Accordion.Header>
              <Accordion.Body>
                {
                cardDetails?.map((cardDetail, index) =>
                (
                  <div className={`${styles.cardBorder} ${cardClass} appointment-card`} key={index}>
                    <Card
                      showHeader={!(header.heading === 'Labs' 
                        && (isLabTestScheduled || (!isLabTestScheduled && index > 0)))}
                      cardDetail={cardDetail}
                    />
                  </div>
                ))
              }
              </Accordion.Body>
            </Accordion.Item>
          </Accordion>
        </div>
      </div>
    </>
  );
};
