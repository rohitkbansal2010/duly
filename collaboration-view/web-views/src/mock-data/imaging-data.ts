import { ImagingDetailsType } from '@types';

export const ImagingData: ImagingDetailsType[] = [ {
  providerDetails: {
    id: 'sc2provider1',
    humanName: { familyName: 'Reilly', givenNames: [ 'John' ], suffixes: [ 'MD' ] },
    photo: {
      contentType: 'image/webp',
      url: 'https://npd.dupagemedicalgroup.com:8444/assets/oam3VAyAIqWVURVvPb4KKllh1U8fLv2Bxrg7TkxJhC8/gravity:sm/resize:fill:800:800:1:1/aHR0cHM6Ly9kbWd3ZWJ0ZXN0c3RvcmFnZS5ibG9iLmNvcmUud2luZG93cy5uZXQvZG1ndGVzdHdlYi9waHlzaWNpYW4taGVhZHNob3RzL1JlaWxseV9Kb2huX09ydGhvcGFlZGljcy0wMDFfd2ViLkpQRw==.webp',
    },
    specialty: 'Physician',
    location: {
      id: '1.87Hts',
      address: {
        addressLine: '133 E. Brush Hill Road',
        addressLine2: 'Suite 100',
        city: 'Elmhurst',
        state: 'Illinois',
        zipCode: '60126',
      },
      geographicCoordinates: { latitude: 41.86151314141414, longitude: -87.93659437373738 },
      phoneNumber: '(630) 893-2210',
      distance: 10,
    },
  },
  tests: [ 'Mammogram', 'CT Scan' ],
},
{
  providerDetails: {
    id: 'sc2provider1',
    humanName: { familyName: 'Ling', givenNames: [ 'Susan' ], suffixes: [ 'MD' ] },
    photo: {
      contentType: 'image/webp',
      url: 'https://npd.dupagemedicalgroup.com:8444/assets/oam3VAyAIqWVURVvPb4KKllh1U8fLv2Bxrg7TkxJhC8/gravity:sm/resize:fill:800:800:1:1/aHR0cHM6Ly9kbWd3ZWJ0ZXN0c3RvcmFnZS5ibG9iLmNvcmUud2luZG93cy5uZXQvZG1ndGVzdHdlYi9waHlzaWNpYW4taGVhZHNob3RzL1JlaWxseV9Kb2huX09ydGhvcGFlZGljcy0wMDFfd2ViLkpQRw==.webp',
    },
    specialty: 'Physician',
    location: {
      id: '1.87Hts',
      address: {
        addressLine: '133 E. Brush Hill Road',
        addressLine2: 'Suite 100',
        city: 'Elmhurst',
        state: 'Illinois',
        zipCode: '60126',
      },
      geographicCoordinates: { latitude: 41.86151314141414, longitude: -87.93659437373738 },
      phoneNumber: '(630) 893-2210',
      distance: 10,
    },
  },
  tests: [ 'Urinalysis' ],
} ];
