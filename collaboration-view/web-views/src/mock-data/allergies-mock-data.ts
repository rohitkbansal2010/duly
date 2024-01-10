import { AllergiesData } from '@types';

export const allergiesMockData: AllergiesData[] = [
  {
    id: 'b5cc803b9a764b6a9e4f23727bdd86a1',
    title: 'Peanuts',
    recorded: '2022-02-21T00:00:00Z',
    categories: [ 'Food' ],
    reactions: [
      {
        title: 'Anaphylaxis',
        severity: 'Severe',
      },
    ],
  },
  {
    id: 'f6f9e86ff688424aa27cd33607efb2ca',
    title: 'Penicillin',
    recorded: '2022-02-18T00:00:00Z',
    categories: [ 'Medication' ],
    reactions: [
      {
        title: 'Itching',
        severity: 'Moderate',
      },
      {
        title: 'Rush',
        severity: 'Mild',
      },
    ],
  },
  {
    id: 'fe593bffaf934bee9c14985e4064a60f',
    title: 'Latex',
    recorded: '2022-02-14T00:00:00Z',
    categories: [ 'Biologic' ],
    reactions: [
      {
        title: 'Rush',
        severity: 'Mild',
      },
    ],
  },
  {
    id: 'a5b9dbeb6579453ea8dee4b3d82c15a5',
    title: 'Pollen',
    recorded: '2022-02-10T00:00:00Z',
    categories: [ 'Environment' ],
    reactions: [
      {
        title: 'Congestion',
        severity: 'Mild',
      },
      {
        title: 'Rush',
        severity: 'Mild',
      },
    ],
  },
  {
    id: '707bbbe19546446dbbf2453d6d0c354e',
    title: 'Latex',
    recorded: '2022-02-04T00:00:00Z',
    categories: [ 'Biologic', 'Food' ],
    reactions: [
      {
        title: 'Rush',
        severity: 'Mild',
      },
    ],
  },
];
