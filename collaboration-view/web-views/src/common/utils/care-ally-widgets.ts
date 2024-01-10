import { HEIGHT_WIDGET_CONTENT, HEIGHT_WIDGET_ITEM } from '@constants';

export const getCountWidgetItems = () =>
  Math.floor(HEIGHT_WIDGET_CONTENT / HEIGHT_WIDGET_ITEM);
