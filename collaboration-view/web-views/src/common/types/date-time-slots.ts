export type DateTimeSlotsType = {
    time: string,
    displayTime: string,
}

export type GetTimeSlotsRequestDataType = {
    date: Date,
    providerId: string,
    meetingType: string | undefined,
    appointmentId: string,
    stepType: string,
    departmentId?: string,
}

export type GetTimeSlotsAPIResponseType = {
    date: string,
    timeSlots: DateTimeSlotsType[],
}
