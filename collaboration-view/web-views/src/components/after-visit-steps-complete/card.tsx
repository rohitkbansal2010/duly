import { Row, Col, Container } from 'react-bootstrap';

import { Avatar } from '@components/avatar';
import { CardDetailsType } from '@components/cv-checkout-accordian/after-steps-accordian';
import { ACCORDIAN_CARD_WIDTH } from '@constants';

import styles from './accordian.scss';

export type CardType = {
  showHeader: boolean,
  cardDetail: CardDetailsType,
}

export const Card = ({ showHeader, cardDetail }: CardType) =>
  (
    <div className={styles.card}>
      {showHeader
      && (
        <div className={styles.cardHeader}>
          <Container>

            <div className={styles.detailsBar}>
              <Row>
                {cardDetail.cardHeader?.map((header, index) => {
                  if (header.dateAndTime) {
                    return (
                      <Col key={index} className={styles.cardHeaderDate}>
                        {header.dateAndTime}
                      </Col>
                    );
                  } else if (header.providerDetails) {
                    return (
                      <Col key={index}>
                        <div className={styles.cardHeaderFlex}>
                          <Avatar
                            src={header.providerDetails.photo}
                            alt={header.providerDetails.name}
                            width={ACCORDIAN_CARD_WIDTH}
                          />
                          <div className={styles.ProviderName}>{header.providerDetails.name}</div>
                        </div>
                      </Col>
                    );
                  }
                })}
              </Row>
            </div>
          </Container>
        </div>
      )}
      <div className={styles.cardBody}>
        <Container>
          <Row>
            <div className={styles.detailsHeading}>
              {cardDetail.cardColumns.map((cardColumn, index) =>
            (
              <Col className={styles.detailsHeadingCol} key={index}>
                <Row className={styles.detailsHeadingKey}>
                  {cardColumn.topLine}
                </Row>
                {cardColumn.middleLine ?
                  (
                    <Row className={styles.detailsHeadingData}>
                      {cardColumn.middleLine}
                    </Row>
                  )
                  : ''
                }
                {
                  cardColumn.bottomLine ?
                    (
                      <Row className={`${styles.bottomLine} pincode-line`}>
                        {cardColumn.bottomLine}
                      </Row>
                    )
                    : ''
                }
              </Col>
            ))}
            </div>
          </Row>
        </Container>
      </div>

    </div>



  );
