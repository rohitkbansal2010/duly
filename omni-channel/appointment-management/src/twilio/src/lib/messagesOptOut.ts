// eslint-disable-next-line no-shadow
export enum OPTOUT {
  STOP = 'STOP',
  START = 'START',
  HELP = 'HELP'
}

type IncomingSmsStatus = 'received'; // FIXME

// TODO: data from Studio Flow will have lower case first letter
export type IncomingSmsData = {
  SmsSid: string,
  SmsStatus: IncomingSmsStatus,
  AccountSid: string,
  To: string,
  From: string,
  Body: string,
}

const DEFAULT_STOP_WORDS = ['stop', 'cancel', 'end', 'quit', 'stopall', 'unsubscribe'];
const DEFAULT_START_WORDS = ['start', 'unstop', 'yes'];
const DEFAULT_HELP_WORDS = ['help'];

export function getDefaultOptOutType(
  message: IncomingSmsData,
): OPTOUT | undefined {
  const { Body } = message;
  if (DEFAULT_STOP_WORDS.includes(Body)) {
    return OPTOUT.STOP;
  }
  if (DEFAULT_START_WORDS.includes(Body)) {
    return OPTOUT.START;
  }
  if (DEFAULT_HELP_WORDS.includes(Body)) {
    return OPTOUT.HELP;
  }
  return undefined;
}

// For Messaging Service with Advanced Opt-Out enabled
export type IncomingSmsDataForAdvancedOptOut = IncomingSmsData & {
  OptOutType?: OPTOUT // will present if Advanced Opt-Out is enabled for Messaging Service
}

export function getAdvancedOptOutType(
  message: IncomingSmsDataForAdvancedOptOut,
): OPTOUT | undefined {
  return message.OptOutType || undefined;
}

export function getOptOutType(
  message: IncomingSmsData | IncomingSmsDataForAdvancedOptOut,
  advanceOptOut: boolean,
) {
  return advanceOptOut ? getAdvancedOptOutType(message) : getDefaultOptOutType(message);
}
