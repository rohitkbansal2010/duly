import { SystemIcons } from '@enums';

import { PersonalData } from './personal-data';

export type Message = {
  id: number;
  date: string;
  subject: string;
  content: string;
  preview: string;
  isSystemMessage: boolean;
  sender?: PersonalData;
  title?: string;
  systemIconType?: SystemIcons;
};
