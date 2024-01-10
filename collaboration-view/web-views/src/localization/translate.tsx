import { locales } from '@localization/locales';
import { store } from '@redux/store';

export const translate = (page: string, path: string): string => {
  const { locale } = store.getState().LOCALE;
  return locales[locale][page][path] || `${path} - Not found`;
};
