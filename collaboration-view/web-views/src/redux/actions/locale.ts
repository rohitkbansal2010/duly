import { LocaleType, LocaleAction } from './types';

export const switchLocale = (locale: string): LocaleAction =>
  ({
    type: LocaleType.SWITCH_LOCALE,
    payload: { locale },
  });
