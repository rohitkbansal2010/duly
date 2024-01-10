import { ModulesDataType } from '@types';

export const modulesMockData: ModulesDataType[] = [
  {
    details: {
      modules: [
        {
          alias: 'ModuleOverview',
          widgets: [
            { alias: 'WidgetQuestions' },
            { alias: 'WidgetVitals' },
            { alias: 'WidgetGoals' },
            { alias: 'WidgetConditions' },
            { alias: 'WidgetAppointments' },
            { alias: 'WidgetMedications' },
            { alias: 'WidgetAllergies' },
            { alias: 'WidgetImmunizations' },
          ],
        },
        {
          alias: 'ModuleCarePlan',
          widgets: [],
        },
        {
          alias: 'ModuleEducation',
          widgets: [],
        },
        {
          alias: 'ModuleResults',
          widgets: [],
        },
        {
          alias: 'ModuleTelehealth',
          widgets: [],
        },
      ],
    },
    id: 'ExampleNavigationModulesConfigForPatient',
    targetAreaType: 'Navigation',
  },
];
