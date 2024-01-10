import {
  followUpIcon, labsIcon, imagingIcon, addMessageIcon, prescriptionIcon 
} from '@icons';
import { CheckoutHeaderType } from '@types';

export const HeaderMockData: CheckoutHeaderType[] = [
  {
    id: 1,
    img: followUpIcon,
    preTitle: 'FOLLOW-UP DETAILS',
    title: '1 Follow up',
    postTitle: '',
    value: 'schedule-follow-up',
  },
  {
    id: 2,
    img: labsIcon,
    preTitle: 'LABS',
    title: '--',
    postTitle: '',
    value: 'lab',
  },
  {
    id: 3,
    img: imagingIcon,
    preTitle: 'IMAGING',
    title: '--',
    postTitle: '',
    value: 'imaging',
  },
  {
    id: 4,
    img: addMessageIcon,
    preTitle: 'REFERRALS',
    title: '2 Referrals',
    postTitle: '',
    value: 'referrals',
  },
  {
    id: 5,
    img: prescriptionIcon,
    preTitle: 'PRESCRIPTIONS',
    title: '--',
    postTitle: '',
    value: 'prescriptions',
  },
];
