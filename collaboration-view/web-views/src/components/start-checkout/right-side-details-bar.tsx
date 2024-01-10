import { noop } from 'lodash';
import { useState } from 'react';
import { DropdownButton, Dropdown } from 'react-bootstrap';

import { appointmentType } from './checkout-details-bar';
import styles from './checkout-details-bar.scss';

type RightSideDetailsBarType = {
  page: string;
  setSelectedAppType?: React.Dispatch<React.SetStateAction<number>>;
  setSelectedMeetingType?: React.Dispatch<React.SetStateAction<string>>;
  providerType?: string;
}

export const RightSideDetailsBar = (props: RightSideDetailsBarType) => {
  const MeetingType = [ 'In-Person', 'Telehealth' ];

  const [ appointmentTypeTitle, setAppointmentTypeTitle ] = useState(appointmentType[1].name);
  const [ meetingTypeTitle, setMeetingTypeTitle ] = useState(MeetingType[0]);

  if (props.page.includes('Follow-Up Appointments')) {
    return <></>;
  } else {
    if (props.page.includes('Schedule')) {
      return (
        <>
          <style type="text/css">
            {`
              .btn-flat {
                position:relative;
                background-color: white;
                color: black;
                border: 0.0625rem solid black;
                border-radius: 0.25rem;
                margin-top: 0.5rem;
                width: 14.313rem;
                text-align:start;
                font-size:var(--font-size-16px-4px);
                line-height:1.5rem;
                font-weight:700;
                
              }

              .dropdown-toggle::after {
                position: absolute;
                top : 40%;
                right: 10px;
                
              }

              .dropdown-item{
                font-size: 1rem !important;
                background-color: white !important;
                color: black !important;
              }
              .dropdown-menu.show {
                transform: unset !important;
                inset: unset !important;
                width: 100%;
              }

            `}
          </style>
          <div className={styles.appointment}>
            <div className={styles.appointmentTitle}>APPOINTMENT TYPE</div>
            <div className={styles.data}>
              <DropdownButton
                id="AppointmentType"
                data-automation="appointment-type"
                title={appointmentTypeTitle}
                className={styles.dropdownStyle}
                variant="flat"
              >
                {appointmentType.map((item, index) =>
                (
                  <Dropdown.Item
                    key={index}
                    bsPrefix="dropdown-item"
                  >
                    <div
                      role="none"
                      onKeyDown={noop}
                      onClick={() => {
                        props.setSelectedAppType && props.setSelectedAppType(item.val);
                        setAppointmentTypeTitle(item.name);
                      }}
                      className={styles.dropdownDiv}
                    >

                      {item.name}
                    </div>
                  </Dropdown.Item>
                ))}
              </DropdownButton>
            </div>
          </div>
          <div className={styles.appointmentFormat}>
            <div className={styles.formatTitle}>APPOINTMENT FORMAT</div>
            <div className={styles.data}>
              <DropdownButton
                id="MeetingType"
                data-automation="appointment-type"
                title={meetingTypeTitle}
                className={styles.dropdownStyle}
                variant="flat"
              >
                {MeetingType.map((item, index) =>
                (
                  <Dropdown.Item
                    key={index}
                    bsPrefix="dropdown-item"
                  >
                    <div
                      role="none"
                      onKeyDown={noop}
                      onClick={() => {
                        setMeetingTypeTitle(item);
                        props.setSelectedMeetingType && props.setSelectedMeetingType(item);
                      }}
                      className={styles.dropdownDiv}
                    >

                      {item}
                    </div>
                  </Dropdown.Item>
                ))}
              </DropdownButton>
            </div>
          </div>
        </>
      );
    } else {
      return (
        <>
          <div className={styles.referralappointment}>
            <div className={styles.appointmentTitle}>REFERRAL VISIT</div>
            <div className={styles.data}>
              {props.providerType ? props.providerType : 'Endocrinologist'}
            </div>
          </div>
          <div className={styles.referralappointmentFormat}>
            <div className={styles.formatTitle}>APPOINTMENT DURATION</div>
            <div className={styles.data}>30 Minutes</div>
          </div>
        </>
      );
    }
  }
};
