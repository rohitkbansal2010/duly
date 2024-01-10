type WidgetType = {
	icon: string;
	title: string;
	col: number;
	row: number;
	counter?: number;
	subtitle?: string;
}

export const careAllyWidgets: WidgetType[] = [
  {
    icon: 'bi bi-clock',
    title: 'upcoming',
    counter: 4,
    subtitle: 'all',
    col: 1,
    row: 1,
  },
  {
    icon: 'bi bi-chat',
    title: 'communication',
    col: 1,
    row: 2,
  },
  {
    icon: 'bi bi-exclamation-triangle',
    title: 'alerts',
    counter: 17,
    subtitle: 'view all',
    col: 2,
    row: 1,
  },
  {
    icon: 'bi bi-file-earmark-minus',
    title: 'prior auths',
    counter: 3,
    subtitle: 'view all',
    col: 2,
    row: 2,
  },
  {
    icon: 'bi bi-check-circle',
    title: 'tasks',
    counter: 12,
    subtitle: 'view all',
    col: 3,
    row: 1,
  },
  {
    icon: 'bi bi-people',
    title: 'new patients',
    counter: 2,
    subtitle: 'view all',
    col: 3,
    row: 2,
  },
];

export const HEIGHT_WIDGET_CONTENT = 333; // TODO: It will have fixed when task 145 is completed
export const HEIGHT_WIDGET_ITEM = 68;
export const LIMIT_COUNT_WIDGET_ITEMS = 4; // This is temporary so far
