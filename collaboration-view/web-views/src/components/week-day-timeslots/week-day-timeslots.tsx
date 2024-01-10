import { useEffect, useState } from 'react';

import { FadedScroll } from '@components/faded-scroll-2';
import { WeekDayTimeslot } from '@components/week-day-timeslot';
import { sunfullIcon, sunriseIcon } from '@icons';
import { DateTimeSlotsType, ReferralData } from '@types';

import styles from './week-day-timeslots.scss';

type WeekDayTimeslotsProps = {
  slotsData: DateTimeSlotsType[];
  selectedTimeslot: string;
  setSelectedTimeslot: React.Dispatch<React.SetStateAction<string>>;
  selectedData?: Array<ReferralData>;
  date: Date;
};

export const WeekDayTimeslots = ({
  slotsData,
  selectedTimeslot,
  setSelectedTimeslot,
  selectedData,
  date,
}: WeekDayTimeslotsProps) => {
  const [ slotsArray, setSlotsArray ] = useState<string[][]>([ [], [], [] ]);
  const selectTimeSlot = (timeslot: string) => {
    setSelectedTimeslot(timeslot);
  };
  const partsOfDay = [ 'Morning', 'Afternoon', 'Evening' ];
  const slotsFilter = () => {
    const finalArray: string[][] = [ [], [], [] ];
    slotsData?.forEach((slot) => {
      if (slot.displayTime.substring(6) === 'AM') {
        finalArray[0].push(slot.displayTime);
      } else if (
        parseInt(slot.displayTime.substring(0, 2)) < 6 ||
        parseInt(slot.displayTime.substring(0, 2)) === 12
      ) {
        finalArray[1].push(slot.displayTime);
      } else {
        finalArray[2].push(slot.displayTime);
      }
    });
    setSlotsArray(finalArray);
  };
  useEffect(() => {
    slotsFilter();
  }, [ slotsData ]);
  return (
    <FadedScroll height="22rem" className={styles.weekDayTimeslotsItems}>
      {slotsArray.map((slots, index) =>
      (
        <div key={index}>
          { slots.length !== 0 ? (
            <>
              <div className={styles.weekDayTimeslotsItemsHead}>
                <img
                  src={index === 1 ? sunfullIcon : sunriseIcon}
                  alt=""
                  className={styles.weekDayTimeslotsItemsHeadIcon}
                />
                <div className={styles.weekDayTimeslotsItemsHeadTitle}>{partsOfDay[index]}</div>
                <div className={styles.weekDayTimeslotsItemsHeadNumber}>({slots.length} slots)</div>
              </div>
              <div className={styles.weekDayTimeslotsItemsSlots}>
                {slots.filter((slot)=>{
              let check = false;
              selectedData?.forEach((selectedDataItem)=>{
                if(slot === selectedDataItem.time 
                  && date.getDate() === selectedDataItem.date?.getDate()){
                  check = true;
                }
              });
              if(check){
                return false;
              }
              return true;
            }).map(timeslot =>
            (
              <WeekDayTimeslot
                data-automation={`time-slots${timeslot}`}
                key={timeslot}
                time={timeslot}
                isSelectedTimeslot={timeslot === selectedTimeslot}
                onSelectTimeSlot={() => {
                  selectTimeSlot(timeslot);
                }}
              />
            ))}
              </div>
            </>
) : ''}
        </div>
      ))}
      {/* </div> */}
    </FadedScroll>
  );
};
