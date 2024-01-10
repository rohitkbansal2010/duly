export enum BaseRoutes {
  care_ally = '/care-ally',
  patient = '/patient',
  collaboration_view = '/',
}

export enum QuickLinkRoutes {
  calendar = '/calendar',
  messages = '/messages',
  test_results = '/test-results',
  health_summary = '/health-summary',
  medications = '/medications',
  symptoms = '/symptoms',
  providers = '/providers',
}

export enum HealthJourneyRoutes {
  AFTER_VISIT_SUMMARY = '/after-visit-summary',
}

export enum AppointmentModulesRoutes {
  MODULE_OVERVIEW = '/overview',
  MODULE_CARE_PLAN = '/care-plan',
  MODULE_EDUCATION = '/education',
  MODULE_RESULTS = '/results',
}

export enum AppointmentModuleWidgetsRoutes {
  QUESTIONS = 'questions',
  VITALS = 'vitals',
  GOALS = 'goals',
  CONDITIONS = 'conditions',
  TIMELINE = 'timeline',
  MEDICATIONS = 'medications',
  ALLERGIES = 'allergies',
  IMMUNIZATIONS = 'immunizations',
}

export const CollaborationViewRoutes = {
  home: BaseRoutes.collaboration_view,
  welcome: `${BaseRoutes.collaboration_view}welcome-page`,
  appointment: `${BaseRoutes.collaboration_view}appointment`,
  startAppointment: (...params: string[]) =>
    `${BaseRoutes.collaboration_view}appointment/${params[0]}/${params[1]}/${params[2]}${AppointmentModulesRoutes.MODULE_OVERVIEW}`,
  startCheckout: `${BaseRoutes.collaboration_view}start-checkout/:appointmentId/:patientId/:practitionerId`,
  examRoom1: `${BaseRoutes.collaboration_view}exam-room/schedule-follow-up`,
};

export const CareAllyRoutes = {
  home: BaseRoutes.care_ally,
  encounter: `${BaseRoutes.care_ally}/patient-360/:patientId/encounter`,
};

export const PatientRoutes = {
  home: BaseRoutes.patient,
  patient_calendar: `${BaseRoutes.patient}${QuickLinkRoutes.calendar}`,
  messages: `${BaseRoutes.patient}${QuickLinkRoutes.messages}`,
  health_summary: `${BaseRoutes.patient}${QuickLinkRoutes.health_summary}`,
  medications: `${BaseRoutes.patient}${QuickLinkRoutes.medications}`,
  symptoms: `${BaseRoutes.patient}${QuickLinkRoutes.symptoms}`,
  providers: `${BaseRoutes.patient}${QuickLinkRoutes.providers}`,
  after_visit_summary: `${BaseRoutes.patient}${HealthJourneyRoutes.AFTER_VISIT_SUMMARY}`,
};

export enum NonAuthRoutes {
  login = '/patient-login',
  care_ally_login = '/care-ally-login',
  cv_login = '/',
  page_not_found = '/page-not-found',
}
