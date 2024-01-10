import { GoalStatus } from '@enums';

export type ProgressSummaryGoal = {
  title: string;
  summary: string;
  target: number;
  achievement: number;
  measure: string;
  status: GoalStatus;
};
