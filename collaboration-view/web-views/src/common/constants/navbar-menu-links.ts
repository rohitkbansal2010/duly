import { UserRoles } from '@enums';

type NavbarMenuType = {
  [key in UserRoles]: {
    links: Array<{
      label: string;
      route: string;
    }>;
  };
}

// mack-nav
export const navbarMenu: NavbarMenuType = {
  [UserRoles.PATIENT]: {
    links: [
      { label: 'Overview', route: '/overiew' },
      { label: 'My Patients', route: '/my-patients' },
      { label: 'DHC Directory', route: '/dmg-directory' },
    ],
  },
  [UserRoles.CARE_ALLY]: {
    links: [
      { label: 'Overview', route: '/overiew' },
      { label: 'My Patients', route: '/my-patients' },
      { label: 'DHC Directory', route: '/dmg-directory' },
    ],
  },
};
