import { physcian } from '@images';



export const CheckoutDetailsBar = {
  profileImage: physcian,
  doctorType: 'Physcian',
  doctorName: 'Dr. Ling',
  providerName: 'PCP',
  location: '1133 South Blvd',
  pincode: 'Oak Park, IL 60302',
  page: 'Schedule',
};


export const FollowUpDetails = [
  {
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
];
