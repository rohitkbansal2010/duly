import { StepperTitles } from '@enums';
import { 
  labsIcon, medicationLightBlueIcon, prescriptionIcon, addMessageIcon, Calendar
} from '@icons';
import { physcian } from '@images';


export type HeaderDataType = {
  icon: string;
  heading: string;
}

export const followUpHeaderData: HeaderDataType = {
  icon: Calendar,
  heading: 'Follow-Up Appointment',
};

export const labsHeaderData: HeaderDataType = {
  icon: labsIcon,
  heading: 'Labs',
};

export const imagingHeaderData: HeaderDataType = {
  icon: labsIcon,
  heading: 'Imaging',
};

export const referralHeaderData: HeaderDataType = {
  icon: addMessageIcon,
  heading: 'Referrals',
};

export const prescriptionHeaderData: HeaderDataType = {
  icon: medicationLightBlueIcon,
  heading: 'Prescription',
};

export const Prescriptions = [
  {
    header:{
      heading: 'Prescription',
      icon: prescriptionIcon,
      message: '3 New Prescription have been sent to your pharmacy.',
    },
    details:[
      {
        medicationName: 'Laxis Tablet 40 mg',
        medicationType: 'RX',
        prescriberName: 'Dr. Ling',
        prescriberImg: physcian,
        instructions: 'Take 2 Tablets every 12 hours',
        reason: 'For leg swelling (COPD)',
      },
      {
        medicationName: 'Pottasium Chloride ER Capsule 10 mEq',
        medicationType: 'RX',
        prescriberName: 'Dr. Ling',
        prescriberImg: physcian,
        instructions: 'Take 1 caplet every 6 hours',
        reason: 'For Potassium replacement (COPD)',
      },
      {
        medicationName: 'Loperamide Chewable Tablet 40 mg',
        medicationType: 'RX',
        prescriberName: 'Dr. Ling',
        prescriberImg: physcian,
        instructions: 'Take 2 Tablets every 8 hours',
        reason: 'Chronic Diarrhea',
      },
    ],
    pharmacyInfo:{
      name:'Walgreens',
      address:'2205 22nd St, Oak Park, IL 60302',
      phone:'(630) 928-0386',
      closingTime:'7PM',
    },
  },
];
export type StepsListDataType = {
  isSkeletonShown: boolean,
  alias: string,
}

export const StepsListData = (skeleton: boolean): StepsListDataType[] => 
  [
    {
      isSkeletonShown: skeleton,
      alias: StepperTitles.FOLLOW_UP,
    },
    {
      isSkeletonShown: skeleton,
      alias: StepperTitles.LABS,
    },
    {
      isSkeletonShown: skeleton,
      alias: StepperTitles.IMAGING,
    },
    {
      isSkeletonShown: skeleton,
      alias: StepperTitles.REFERRAL,
    },
    {
      isSkeletonShown: skeleton,
      alias: StepperTitles.PRESCRIPTION,
    },
    {
      isSkeletonShown: false,
      alias: StepperTitles.LAB_LOCATIONS,
    },
    {
      isSkeletonShown: false,
      alias: StepperTitles.IMAGING_LOCATION,
    },
    {
      isSkeletonShown: false,
      alias: StepperTitles.PREFERRED_PHARMACY,
    },
  ];

