export type ReferralFollowUp = {
    id: string,
    date: Date,
    time: string,
    location: string,
    pincode: string,
    referralType: string,
    duration: string,
    providerInfo: {
        name: string,
        icon: string
    }
};
