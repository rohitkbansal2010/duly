import { LabTestDetailsType } from '@types';

export const LabsData: LabTestDetailsType = {
  providerDetails: {
    id: 'sc2provider1',
    humanName: { familyName: 'Fitzgerald', givenNames: [ 'Michael', 'E' ], suffixes: [ 'PCP' ] },
    photo: {
      contentType: 'image/jpg',
      url: 'https://dmgwebprodstorage.blob.core.windows.net/dmgprodweb/physician-headshots/Fitzgerald_Michael_FM-003websize.jpg',
    },
    specialty: 'Physician',
    location: {
      id: 'dsJKHasd.87Hts',
      address: {
        addressLine: '1121 South Blvd',
        addressLine2: 'Suite 100',
        city: 'Oak Park',
        state: 'Illinois',
        zipCode: '60302',
      },
      geographicCoordinates: { latitude: 41.86151314141414, longitude: -87.93659437373738 },
      phoneNumber: '(630) 893-2210',
      distance: 10,
    },
  },
  tests: [ 'Mammogram', 'CT Scan', 'Comprehensive Metabolic Panel', 'Urinalysis' ],
};

export const LabsLocation = [
  {
    id: '111',
    name: 'Naperville',
    location: {
      address: {
        addressLine: '1888 Bay Scott Circle',
        city: 'Naperville',
        state: 'IL',
        zipCode: '60540',
      },
      phoneNumber: '(432) 740-7232',
      workingHours: '8 AM - 6 PM',
      distance: 3,
    },
  },
  {
    id: '112',
    name: 'Wheaton',
    location: {
      address: {
        addressLine: '303 W Wesley Street',
        city: 'Wheaton',
        state: 'IL',
        zipCode: '60187',
      },
      phoneNumber: '(484) 465-7745',
      workingHours: '8 AM - 7 PM',
      distance: 10,
    },
  },
  {
    id: '113',
    name: 'Lombard',
    location: {
      address: {
        addressLine: '1987 Oak Brook',
        city: 'Lombard',
        state: 'IL',
        zipCode: '60137',
      },
      phoneNumber: '(484) 465-7745',
      workingHours: '8 AM - 7 PM',
      distance: 13.6,
    },
  },
  {
    id: '114',
    name: 'Oak Park',
    location: {
      address: {
        addressLine: '1133 South Blvd',
        city: 'Oak Park',
        state: 'IL',
        zipCode: '60302',
      },
      phoneNumber: '(630) 928-0386',
      workingHours: '9 AM - 7 PM',
      distance: 20,
    },
  },
  {
    id: '115',
    name: 'Cook Count',
    location: {
      address: {
        addressLine: '366 Downers Grove',
        city: 'Cook Count',
        state: 'IL',
        zipCode: '60004',
      },
      phoneNumber: '(630) 928-0386',
      workingHours: '9 AM - 7 PM',
      distance: 31,
    },
  },
  {
    id: '116',
    name: 'St. Charles',
    location: {
      address: {
        addressLine: '1125 North Arck',
        city: 'St. Charles',
        state: 'IL',
        zipCode: '60119',
      },
      phoneNumber: '(043) 987-1074',
      workingHours: '9 AM - 6 PM',
      distance: 287,
    },
  },
];
