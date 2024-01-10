import { TimeSlotType } from '@types';

export const MOCK_TIME_SLOTS: Array<TimeSlotType[]> = [
  [],
  [
    { time: '8:45 am', id: 1 },
    { time: '2:00 pm', id: 2 },
    { time: '1:45 pm', id: 3 },
  ],
  [
    { time: '8:45 am', id: 4 },
    { time: '1:00 pm', id: 5 },
  ],
  [ { time: '2:15 pm', id: 6 } ],
  [
    { time: '8:45 am', id: 7 },
    { time: '10:00 am', id: 8 },
    { time: '2:15 pm', id: 9 },
  ],
  [
    { time: '10:00 am', id: 10 },
    { time: '11:15 am', id: 11 },
    { time: '2:15 pm', id: 12 },
  ],
  [],
];
