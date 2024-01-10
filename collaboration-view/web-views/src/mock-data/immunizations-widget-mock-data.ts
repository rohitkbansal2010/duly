import { ProgressPanelStatuses } from '@enums';
import { ImmunizationsWidgetDataType } from '@types';

export const immunizationsWidgetMockData: ImmunizationsWidgetDataType = {
  progress: {
    percentageCompletion: 75,
    recommendedGroupNumber: 4,
    completionStatus: ProgressPanelStatuses.ALMOST,
  },
  recommendedImmunizations: [
    {
      title: 'TDAP',
      vaccinations: [
        {
          title: 'Tdap',
          status: 'Completed',
          dateTitle: 'ADMINISTRATED',
          date: '3/11/2002',
        },
        {
          title: 'Tdap',
          status: 'Completed',
          dateTitle: 'ADMINISTRATED',
          date: '3/21/2012',
        },
        {
          title: 'Tdap',
          status: 'DueOn',
          dateTitle: 'DUE',
          date: '3/15/2022',
        },
      ],
    },
    {
      title: 'FLU',
      vaccinations: [
        {
          title: 'Influenza, seasonal, injectable',
          status: 'Addressed',
          dateTitle: 'ADMINISTRATED',
          date: '9/3/2017',
        },
        {
          title: 'Influenza, seasonal, injectable',
          status: 'Addressed',
          dateTitle: 'ADMINISTRATED',
          date: '9/5/2018',
        },
        {
          title: 'Influenza, seasonal, injectable',
          status: 'Addressed',
          dateTitle: 'ADMINISTRATED',
          date: '9/7/2019',
        },
        {
          title: 'Influenza, high dose seasonal',
          status: 'Completed',
          dateTitle: 'ADMINISTRATED',
          date: '9/1/2020',
        },
        {
          title: 'Influenza, high dose seasonal',
          status: 'Postponed',
          dateTitle: 'POSTPONED',
        },
      ],
    },
    {
      title: 'Encephalitis',
      vaccinations: [
        {
          title: 'IXIARO',
          status: 'Completed',
          dateTitle: 'ADMINISTRATED',
          date: '5/7/2018',
        },
        {
          title: 'IXIARO',
          status: 'Postponed',
          dateTitle: 'POSTPONED',
          date: '5/11/2019',
          notes: 'Patient refused second dose on 5/11/2019 as she noticed she felt like the first dose gave her anxiety and made her feel dizzy.',
        },
      ],
    },
    {
      title: 'COVID-19',
      vaccinations: [
        {
          title: 'Covid-19 Vaccine Pfizer 30 mcg/0.3 ml',
          status: 'Completed',
          dateTitle: 'ADMINISTRATED',
          date: '3/17/2021',
        },
        {
          title: 'Covid-19 Vaccine Pfizer 30 mcg/0.3 ml',
          status: 'Completed',
          dateTitle: 'ADMINISTRATED',
          date: '5/17/2021',
        },
        {
          title: 'Covid-19 Vaccine Pfizer 30 mcg/0.3 ml',
          status: 'DueSoon',
          dateTitle: 'DUE',
        },
      ],
    },
    {
      title: 'ADENO',
      vaccinations: [
        {
          title: 'Adenovirus types 4 and 7',
          status: 'DueSoon',
          dateTitle: 'DUE',
        },
      ],
    },
    {
      title: 'RABIES',
      vaccinations: [
        {
          title: 'Rabies - IM fibroblast culture',
          status: 'NotDue',
          dateTitle: 'DUE',
        },
      ],
    },
  ],
  pastImmunizations: [
    {
      title: 'HepA',
      vaccinations: [
        {
          title: 'Hep A, adult',
          dateTitle: 'ADMINISTRATED',
          date: '7/5/1985',
          dose: {
            amount: 1,
            unit: 'mL',
          },
        },
      ],
    },
    {
      title: 'HepB',
      vaccinations: [
        {
          title: 'Hep B-dialysis',
          dateTitle: 'ADMINISTRATED',
          date: '2/15/1998',
          dose: {
            amount: 1,
            unit: 'mL',
          },
        },
      ],
    },
    {
      title: 'MENING',
      vaccinations: [
        {
          title: 'Meningococcal MCV4O',
          dateTitle: 'NOT ADMINISTRATED',
        },
      ],
    },
    {
      title: 'POLIO',
      vaccinations: [
        {
          title: 'IPV (e-IPV)',
          dateTitle: 'ADMINISTRATED',
        },
      ],
    },
  ],
};
