import { useCallback } from 'react';

import { locales } from '@localization/locales';

type ReturnType = {
  translate: (page: string, path: string) => string;
};

export const useTranslate = (locale: string): ReturnType => {
  const translate = useCallback(
    (page: string, path: string): string => 
      locales[locale][page][path] || `${path} - Not found`,
    [ locale ]
  );

  return { translate };
};
