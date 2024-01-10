import { Calendar, labsIcon, prescriptionIcon } from '@icons';
import { physcian } from '@images';

type HeaderDataType = {
  icon: string;
  heading: string;
  date:string;
}

const headerData: HeaderDataType = {
  icon: Calendar,
  heading: 'Follow-Up Appointment',
  date: 'Friday, Mar 3, 2022',
};

const labsHeaderData:HeaderDataType = {
  icon: labsIcon,
  heading: 'Labs',
  date: '2 labs & 1 imaging',
};

export const AfterVisitsData = {
  followUpAppointments:[
    {
      headerData:
        headerData,
      scheduleDetails:{
        date:'Fri, Mar 3, 2022',
        time:'11:20 AM',
        doctorImage: physcian,
        doctorName: 'Dr. Ling',
      },
      appointmentDetails:[
        { location: '1133 South Blvd Oak Park, IL 60302' },
        { appointment_type: '6-month Exam' },
        { duration: '30 Minutes' },
      ],
    },
  ],
  Labs:[
    {
      headerData: labsHeaderData,
      scheduleDetails:{
        date:'Fri, Mar 3, 2022',
        time:'11:20 AM',
      },
      appointmentDetails:[
        { location: '1133 South Blvd Oak Park, IL 60302' },
        { appointment_type: 'Comprehensive Metabolic Panel' },
      ],
    },
  ],
  Imaging:[],
  Referrals:[],
  Prescriptions:[
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
  ],
  Billings:[],
};
