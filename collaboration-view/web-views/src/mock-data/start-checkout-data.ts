import { userImageIcon, qrCodeIcon } from '@icons';
import { StartCheckout } from '@types';

export const StartCheckoutData: StartCheckout = {
  name: 'Ana Maria',
  fullName: 'Ana Maria Reyes',
  description: 'If you’d like to receive follow-ups after this visit, scan the QR code below.',
  phone: '(773) 404-2827',
  email: 'ana.reyes@gmail.com',
  profilePic: userImageIcon,
  qrCode: qrCodeIcon,
};
