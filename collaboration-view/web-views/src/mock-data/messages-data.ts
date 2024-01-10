import { SystemIcons } from '@enums';
import { Message } from '@types';

import { pickPDById } from './personal-data';

export const messages: Message[] = [
  {
    id: 2001,
    date: '06/14/2021',
    subject: 'Your Latest Results',
    content: `Dear Ana,  Cras mattis consectetur purus sit amet fermentum. Maecenas sed diam eget risus varius blandit sit amet non magna. Vestibulum id ligula porta felis euismod semper. Praesent commodo cursus magna, vel scelerisque nisl consectetur et. Cras justo odio, dapibus ac facilisis in, egestas eget quam. Duis mollis, est non commodo luctus, nisi erat porttitor ligula, eget lacinia odio sem nec elit.  Sed posuere consectetur est at lobortis. Integer posuere erat a ante venenatis dapibus posuere velit aliquet. Vestibulum id ligula porta felis euismod semper. Cras justo odio, dapibus ac facilisis in, egestas eget quam.`,
    preview: 'Integer posuere erat a ante dapibus posuere velit. Aenean lacinia biben…',
    isSystemMessage: false,
    sender: pickPDById(201),
  },
  {
    id: 2002,
    date: '06/14/2021',
    subject: 'After Visit Summary',
    content: 'Dear Duncan,  Integer posuere erat a ante dapibus posuere velit. Aenean lacinia' +
      ' biben…',
    preview: 'Integer posuere erat a ante dapibus posuere velit. Aenean lacinia biben…',
    isSystemMessage: false,
    sender: pickPDById(202),
    title: 'Dr.',
  },
  {
    id: 2003,
    date: '06/14/2021',
    subject: 'Medication Refill',
    content: 'Dear User,  Integer posuere erat a ante dapibus posuere velit. Aenean lacinia biben…',
    preview: 'Integer posuere erat a ante dapibus posuere velit. Aenean lacinia biben…',
    isSystemMessage: true,
    systemIconType: SystemIcons.MEDICINE_BOTTLE_DARK_GRAY,
  },
  {
    id: 2004,
    date: '06/14/2021',
    subject: 'Appointment',
    content: 'Dear User,  Integer posuere erat a ante dapibus posuere velit. Aenean lacinia biben…',
    preview: 'Integer posuere erat a ante dapibus posuere velit. Aenean lacinia biben…',
    isSystemMessage: true,
    systemIconType: SystemIcons.CALENDAR_DARK_GRAY,
  },
];
