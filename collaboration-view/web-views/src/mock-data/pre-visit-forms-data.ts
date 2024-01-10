import { MockQuestionType } from '@types';

const MOCK_QUESTIONS_LABELS: string[] = [
  'my diet',
  'my symptoms',
  'exercise',
  'traveling with my condition',
  'my medications',
  'support groups',
];

export const QUESTIONNAIRE_QUESTIONS: MockQuestionType[] =
  MOCK_QUESTIONS_LABELS.concat('').map(label =>
    ({
      label,
      checked: false,
    }));
