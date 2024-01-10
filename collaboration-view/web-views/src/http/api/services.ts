const BASE_URL = window?.env?.BASE_URL || '';
const ENCOUNTER_API = `${BASE_URL}/encounter`;
const RESOURCE_API = `${BASE_URL}/resource`;

export const EncouterServices = {
  PATIENTS: `${ENCOUNTER_API}/v1/patients`,
  PRACTITIONERS: `${ENCOUNTER_API}/v1/practitioners`,
  SITES: `${ENCOUNTER_API}/v1/sites`,
  APPOINTMENTS: `${ENCOUNTER_API}/v1/appointments`,
  APPOINTMENTS_FOR_SAME_PATIENT: `${ENCOUNTER_API}/v1/Appointments`,
  TEST_REPORTS: `${ENCOUNTER_API}/v1/TestReports`,
  USERS: `${ENCOUNTER_API}/v1/Users`,
  CHECKOUT_APPOINTMENT: `${ENCOUNTER_API}/v1`,
} as const;

export const ResourceServices = { CONFIGURATIONS: `${RESOURCE_API}/v1/Configurations` } as const;
