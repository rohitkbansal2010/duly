import { GoalStatus } from '@enums';
import { ProgressSummaryGoal } from '@types';

export const progressSummaryGoals: ProgressSummaryGoal[] = [
  {
    title: 'Exercising More?',
    summary: 'I want to exercise at least 3-4 times per week so that I can spend more time' +
      ' outside with my grandkids.',
    target: 30,
    achievement: 32,
    measure: 'min',
    status: GoalStatus.GOAL_MET,
  },
  {
    title: 'More steps?',
    summary: 'I want to walk more so that I can spend more time with my more active friends, and' +
      'have less shortness of breath.',
    target: 4,
    achievement: 3.5,
    measure: 'k',
    status: GoalStatus.IMPROVING,
  },
  {
    title: 'Oximeter Results',
    summary: 'As a results of my increased activity and other lifestyle improvements, I want to' +
      ' improve my Oximeter results.',
    target: 92,
    achievement: 90,
    measure: '%',
    status: GoalStatus.PROGRESS,
  },
];
