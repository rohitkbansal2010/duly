import { PractitionerRole, CareTeamMemberRole } from '@enums';

import { HumanName, Photo } from './member';

export type ActiveUser = {
  id: string;
  name: HumanName;
  photo?: Photo;
  role?: PractitionerRole | CareTeamMemberRole;
}
