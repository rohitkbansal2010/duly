import { CommunicationType } from '@enums';

import { PersonalData } from './personal-data';

export type CommunicationMessage = {
  messageId: number;
  incoming: boolean;
  counterpart: PersonalData;
  time: string;
  via: CommunicationType;
  recipient: string[];
  content: string;
  viewed: boolean;
  replyTo?: number;
};
