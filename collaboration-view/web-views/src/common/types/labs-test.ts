import { ProviderDetailsType } from './cv-checkout-appointments';

export type Location = {
    id: string,
    name: string,
    location: {
        address: {
          addressLine: string,
          city: string,
          state: string,
          zipCode: string,
        },
        phoneNumber: string,
        workingHours: string,
        distance: number,
    };
    lat?: string,
    lng?: string,
}

export type NearbyLabLocation = {
    labId: number,
    labName: string,
    externalLabYn: string,
    labLlbId: string,
    llbName: string,
    llbAddressLn1: string,
    llbAddressLn2: string,
    llCity: string,
    llState: string,
    llZip: string,
    labLatitude: number,
    labLongitude: string,
    distance: string
}

export type LabOrImagingTestDetailsType = {
    testOrder: [{
        orderName: string,
        authoredOn: string
    }],
    orderCount: number
}

export type ImagingDetailsType = {
    providerDetails: ProviderDetailsType;
    tests: string[];
};
