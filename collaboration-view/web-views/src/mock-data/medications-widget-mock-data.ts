import { MedicationsWidgetDataType } from '@types';

export const mockMedicationsWidgetData: MedicationsWidgetDataType = {
  regular: [
    {
      id: 'Regular_Medication_Id',
      scheduleType: 'Regular',
      title: 'Potassium chloride ER Capsule 10 mEq second',
      reason: 'For potassium replacement (COPD)',
      startDate: '2022-02-01T00:00:00Z',
      provider: {
        familyName: 'Reyes',
        givenNames: [
          'Ana',
          'Maria',
        ],
        prefixes: [
          'Dr.',
        ],
      },
      instructions: 'Take 1 capsule by month 2 times a day',
    },
    {
      id: 'Regular_Medication_Id2',
      scheduleType: 'Regular',
      title: 'Potassium chloride ER Capsule 10 mEq first',
      reason: 'For potassium replacement (COPD)',
      startDate: '2022-02-01T00:00:00Z',
      provider: {
        familyName: 'Reyes',
        givenNames: [
          'Ana',
          'Maria',
        ],
        prefixes: [
          'Dr.',
        ],
      },
      instructions: 'Take 1 capsule by month 2 times a day',
    },
    {
      id: 'Regular_Medication_Id3',
      scheduleType: 'Regular',
      title: 'BPotassium chloride ER Capsule 10 mEq fourth',
      reason: 'For potassium replacement (COPD)',
      provider: {
        familyName: 'Reyes',
        givenNames: [
          'Ana',
          'Maria',
        ],
        prefixes: [
          'Dr.',
        ],
      },
      instructions: 'Take 1 capsule by month 2 times a day',
    },
    {
      id: 'Regular_Medication_Id4',
      scheduleType: 'Regular',
      title: 'APotassium chloride ER Capsule 10 mEq third',
      reason: 'For potassium replacement (COPD)',
      provider: {
        familyName: 'Reyes',
        givenNames: [
          'Ana',
          'Maria',
        ],
        prefixes: [
          'Dr.',
        ],
      },
      instructions: 'Take 1 capsule by month 2 times a day',
    },
    {
      id: 'Regular_Medication_Id_2',
      scheduleType: 'Regular',
      title: 'CPotassium chloride ER Capsule 10 mEq fifth',
    },
  ],
  other: [
    {
      id: 'Other_Medication_Id5',
      scheduleType: 'Other',
      title: 'ProAir HFA Inhaler 90 mcg/inh',
      reason: 'Breathing medications for (Asthma) attacks',
      startDate: '2022-02-01T00:00:00Z',
      provider: {
        familyName: 'Reyes',
        givenNames: [
          'Ana',
          'Maria',
        ],
        prefixes: [
          'Dr.',
        ],
      },
      instructions: 'For Adults and children 12 years and over: Use this medicine every 4 to 6 hours, or as needed. Inhale one to 2 puffs by month each time.',
    },
    {
      id: 'Other_Medication_Id6',
      scheduleType: 'Other',
      title: 'ProSuperAir HFA Inhaler 90 mcg/inh',
      reason: 'Breathing medications for (Asthma) attacks',
      startDate: '2022-02-01T00:00:00Z',
      provider: {
        familyName: 'Reyes',
        givenNames: [
          'Ana',
          'Maria',
        ],
        prefixes: [
          'Dr.',
        ],
      },
      instructions: 'For Adults and children 12 years and over: Use this medicine every 4 to 6 hours, or as needed. Inhale one to 2 puffs by month each time.',
    },
  ],
};
