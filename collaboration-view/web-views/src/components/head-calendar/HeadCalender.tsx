
import { noop } from 'lodash';
import { useState, useEffect, useRef } from 'react';
import { Tab, Nav } from 'react-bootstrap';
import { useDispatch, useSelector } from 'react-redux';
import { useParams } from 'react-router-dom';

import { NoDataAvailable } from '@components/no-data-available/no-data-available';
import { WeekDayTimeslots } from '@components/week-day-timeslots';
import { noProvider, noSlotData } from '@icons';
import { getWeekDayTimeSlots, setFollowUpDate } from '@redux/actions/cv-checkout-appointments';
import { RootState } from '@redux/reducers';
import { ReferralData } from '@types';

import styles from './HeadCalender.scss';
import 'bootstrap-icons/font/bootstrap-icons.css';
import { TimeslotContainerSkeleton } from './timeslot-container-skeleton';

const datesLength = 11;

const getDate = (start: number) => {
  const today = new Date();
  const dates = [];
  const year = today.getFullYear();
  const month = today.getMonth();
  const date = today.getDate();
  for (let i = start; i < start + datesLength; i++) {
    const day = new Date(year, month, date + i);
    dates.push(day);
  }
  return dates;
};

const days = [ 'Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat' ];
const months = [
  'January',
  'February',
  'March',
  'April',
  'May',
  'June',
  'July',
  'August',
  'September',
  'October',
  'November',
  'December',
];

type HeadCalenderProps = {
  setDate: React.Dispatch<React.SetStateAction<Date>>;
  setTime: React.Dispatch<React.SetStateAction<string>>;
  date: Date;
  selectedAppType?: number;
  selectedMeetingType?: string;
  reset: boolean;
  selectedData?: Array<ReferralData>;
  stepType: string;
  providerId?: string;
  department_Id?: string;
};

export const HeadCalender = ({
  setDate, 
  setTime, 
  date, 
  selectedAppType, 
  selectedMeetingType, 
  reset, 
  selectedData, 
  stepType, 
  providerId,
  department_Id,
}: HeadCalenderProps) => {
  const dispatch = useDispatch();
  const [ startIndex, setStartIndex ] = useState(0);
  const [ dates, setDates ] = useState(getDate(0));
  const [ activeKey, setActiveKey ] = useState('date0');
  const [ toggle, setToggle ] = useState(false);
  const [ selectedTimeslot, setSelectedTimeslot ] = useState('');


  const [ selectedDate, setSelectedDate ] = useState(dates[0]); //changed
  const [ selectedDateHasSlots, setSelectedDateHasSlots ] = useState(true);

  const {
    weekDayTimeSlots,
    isTimeSlotSkeletonShown,
    selectedFollowUpDate,
  } = useSelector(({ CHECKOUT_APPOINTMENTS }: RootState) => 
    CHECKOUT_APPOINTMENTS);
  const { appointmentId } = useParams<{ patientId: string, appointmentId: string }>(); 
  
  
  useEffect(()=>{
    if(selectedFollowUpDate && stepType === 'F'){
      setDates(getDate(-Math.floor(((new Date() - selectedFollowUpDate) / 86400000))));
      setStartIndex(-Math.floor(((new Date() - selectedFollowUpDate) / 86400000)));
    }
  }, []);
    
  useEffect(() => {
    if (stepType === 'R')
      dispatch(getWeekDayTimeSlots({
        date,
        providerId: providerId ? providerId : '',
        meetingType: '',
        appointmentId,
        stepType: stepType ? stepType : '',
        departmentId: department_Id,
      }));
    else if(stepType !== 'F') 
      dispatch(getWeekDayTimeSlots({
        date,
        providerId: providerId ? providerId : '',
        meetingType: '',
        appointmentId,
        stepType: stepType ? stepType : '',
      }));
  }, [ dispatch, date, selectedMeetingType ]);

  useEffect(()=>{
    if (stepType === 'F')
      dispatch(getWeekDayTimeSlots({
        date:selectedFollowUpDate,
        providerId: providerId ? providerId : '',
        meetingType: selectedMeetingType,
        appointmentId,
        stepType: stepType ? stepType : '',
      }));
  }, [ selectedMeetingType, selectedFollowUpDate ]);

  useEffect(() => {
    if (setTime) setTime(selectedTimeslot);
  }, [ selectedTimeslot ]);

  useEffect(() => {
    if (setDate && stepType !== 'F') setDate(selectedDate);
  }, [ selectedDate ]);

  useEffect(() => {
    if (setDate && stepType === 'F') setDate(selectedFollowUpDate);
  }, [ selectedFollowUpDate ]);  


  useEffect(()=>{
    setSelectedTimeslot('');
  }, [ selectedMeetingType ]);

  const useDidMountEffect = (fnc, deps)=>{
    const didMount = useRef(false);
    useEffect(()=>{
      if(didMount.current) fnc();
      else didMount.current = true;
    }, deps);
  };
  useDidMountEffect(()=>{
    if (selectedAppType) {
      const today = new Date();
      const year = today.getFullYear();
      const month = today.getMonth();
      const date = today.getDate();
      const day1 = new Date(year, month, date);
      let day2;
      if (selectedAppType === 0.5) {
        day2 = new Date(year, month, date + 14);        
      } else {
        day2 = new Date(
          year,
          parseInt(month.toString()) + parseInt(selectedAppType.toString()),
          date
        );
      }
    
      const dist = (day2.getTime() - day1.getTime()) / 86400000;
      setDates(getDate(dist));
      setStartIndex(dist);
      setActiveKey('date0');
      setSelectedTimeslot('');
      setSelectedDate(day2); //change
      stepType === 'F' && dispatch(setFollowUpDate(day2));
    }
  }, [ selectedAppType ]);

  const nextWeek = () => {
    setDates(getDate(startIndex + datesLength));
    setStartIndex(startIndex + datesLength);
    setSelectedTimeslot('');
    setSelectedDate(new Date(selectedDate.getTime() + 86400000 * datesLength));
    stepType === 'F' && dispatch(setFollowUpDate(new Date(selectedDate.getTime() + 86400000 * datesLength)));
  };

  const prevWeek = () => {
    if (startIndex !== 0) {
      if (startIndex < datesLength) {
        setDates(getDate(0));
        setStartIndex(0);
        setSelectedDate(new Date());
        stepType === 'F' && dispatch(setFollowUpDate(new Date()));
      } else {
        setDates(getDate(startIndex - datesLength));
        setStartIndex(startIndex - datesLength);
        setSelectedDate(new Date(selectedDate.getTime() - 86400000 * datesLength));
        stepType === 'F' && dispatch(setFollowUpDate(new Date(selectedDate.getTime() - 86400000 * datesLength)));
      }
      
     
      setSelectedTimeslot('');
    }
  };

  const prevMonth = () => {
    if (
      dates[0].getFullYear() === new Date().getFullYear() &&
      (dates[0].getMonth() === new Date().getMonth() ||
        (dates[0].getMonth() === new Date().getMonth() + 1 && dates[0].getDate() === 1))
    ) {
      setDates(getDate(0));
      setStartIndex(0);
      setActiveKey('date0');
      setSelectedTimeslot('');
      setSelectedDate(dates[0]);
      stepType === 'F' && dispatch(setFollowUpDate(dates[0]));
      return;
    }
    const today = dates[0];
    const year = today.getFullYear();
    const month = today.getMonth();
    const date = today.getDate();
    const day1 = new Date(year, month, date);
    let day2;
    if (date !== 1) {
      day2 = new Date(year, month, 1);
    } else {
      day2 = new Date(year, month - 1, 1);
    }
    const dist = (day1.getTime() - day2.getTime()) / 86400000;
    setDates(getDate(startIndex - dist));
    setStartIndex(startIndex - dist);
    setActiveKey('date0');
    setSelectedTimeslot('');
    setSelectedDate(day2); //change
    stepType === 'F' && dispatch(setFollowUpDate(day2));
  };
  const nextMonth = () => {
    const today = dates[0];
    const year = today.getFullYear();
    const month = today.getMonth();
    const date = today.getDate();
    const day1 = new Date(year, month, date);
    const day2 = new Date(year, month + 1, 1);
    const dist = (day2.getTime() - day1.getTime()) / 86400000;
    setDates(getDate(startIndex + dist));
    setStartIndex(startIndex + dist);
    setActiveKey('date0');
    setSelectedTimeslot('');
    setSelectedDate(day2);
    stepType === 'F' && dispatch(setFollowUpDate(day2));
  };
  const appendActiveKeyClass = (className:string):string => 
    className + (selectedDateHasSlots ? ` ${styles.activeTab}` : ` ${styles.noSlots}`);

  
  useEffect(() => {
    if(reset)
    {
      setSelectedTimeslot('');
    }
  }, [ reset ]);
  
  const getTimeSlots = () => {
    if(isTimeSlotSkeletonShown) {
      return <TimeslotContainerSkeleton />;
    }
    if(!weekDayTimeSlots) {
      return (
        <><NoDataAvailable
          height={22} 
          message="Couldn’t get the slots!"
          isMessage={true}
          isMessageDetail={true} 
          messageDetail="Unable to fetch the slots. Please try again or choose a different date."        
          icon={noProvider}
          iconHeight="8.5rem"
          iconWidth="8.5rem"
          isButton={true}
          buttonText="Try Again"
          forSlot={true}
          stepType={stepType}
          selectedMeetingType={selectedMeetingType}
          selectedDate = {date}
          /></>
      );
    }
    if(!weekDayTimeSlots.length && selectedDateHasSlots){
      setSelectedDateHasSlots(false);
    }
    else if(weekDayTimeSlots.length > 0 && selectedDateHasSlots === false){
      setSelectedDateHasSlots(true);
    }
    return !weekDayTimeSlots.length 
      ? (
        <NoDataAvailable
          height={22} 
          message="No slots available for this date! Please choose a different date."
          isMessage={true}
          isMessageDetail={false}         
          icon={noSlotData}           
          iconHeight="11.5rem"
          iconWidth="21.9rem"  
          contentLocation="flex-start"          
        />
      )
      : (
        <WeekDayTimeslots
          slotsData={weekDayTimeSlots}
          selectedTimeslot={selectedTimeslot}
          setSelectedTimeslot={setSelectedTimeslot}
          selectedData={selectedData}
          date={date}
        />
      );
  };

  return (
    <>
      <Tab.Container
        defaultActiveKey="date0"
        onSelect={(key) => {
          setToggle(!toggle);
          setSelectedTimeslot('');
          if (key === null) {
            setActiveKey('date0');
            setSelectedDate(dates[0]); //change
            stepType === 'F' && dispatch(setFollowUpDate(dates[0]));
          } else {
            setActiveKey(key);
            setSelectedDate(dates[parseInt(key.slice(4, key.length))]); //change
            stepType === 'F' && dispatch(setFollowUpDate(dates[parseInt(key.slice(4, key.length))]));
          }
        }}
      >
        <div className={styles.line} style={{ marginTop: '1rem' }} />
        <div className={styles.monthContainer}>
          <div
            role="button"
            onKeyDown={noop}
            tabIndex={-1}
            className={
              startIndex === 0
                ? styles.monthContainerShiftLeftInactive
                : styles.monthContainerShiftLeft
            }
            onClick={() =>
              prevMonth()}
          >
            <span data-automation="calendar-month-left-button">
              <i className="bi bi-chevron-left" />
            </span>
          </div>
          <div className={styles.monthContainerName}>
            {months[dates[0].getMonth()]} {dates[0].getFullYear()}
          </div>
          <div
            role="button"
            onKeyDown={noop}
            tabIndex={-1}
            className={styles.monthContainerShiftRight}
            onClick={() =>
              nextMonth()}
            data-automation="calendar-month-right-button"
          >
            <i className="bi bi-chevron-right" />
          </div>
        </div>
        <div className={styles.line} />
        <div className={styles.dateContainer}>
          <div
            role="button"
            onKeyDown={noop}
            tabIndex={-1}
            className={
              startIndex === 0
                ? styles.dateContainerShiftLeftInactive
                : styles.dateContainerShiftLeft
            }
            onClick={() =>
              prevWeek()}
          >
            <span data-automation="calendar-dates-left-button">
              <i className="bi bi-chevron-left" />
            </span>
          </div>
          <Nav className={styles.dateContainerDates}>
            {dates.map((date: Date, index: number) =>
            (
              <Nav.Item
                className={styles.navbarItem}
                key={`date${index}`}
                data-automation={`dates${index}`}
              >
                <Nav.Link
                  className={
                    `date${index}` === activeKey ? styles.navbarItemActive : styles.navbarItemLink
                  }
                  eventKey={`date${index}`}
                >
                  <div
                    className={
                      `date${index}` === activeKey 
                        ? appendActiveKeyClass(styles.dateContainerDatesDate)
                        : styles.dateContainerDatesDate
                    }
                    key={index}
                  >
                    <div className={styles.dateContainerDatesDateDay}>{days[date.getDay()]}</div>
                    <div className={styles.dateContainerDatesDateNumber}>{date.getDate()}</div>
                  </div>
                </Nav.Link>
              </Nav.Item>
            ))}
          </Nav>
          <div
            role="button"
            onKeyDown={noop}
            tabIndex={-1}
            className={styles.dateContainerShiftRight}
            onClick={() =>
              nextWeek()}
            data-automation="calendar-dates-right-button"
          >
            <i className="bi bi-chevron-right" />
          </div>
        </div>
       
        {getTimeSlots()}
        
      </Tab.Container>
    </>
  );
};
