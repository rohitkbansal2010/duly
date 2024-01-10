import en from './en-US.json';
import es from './es-ES.json';

type LocaleType = {
  [key: string]: {
    [key: string]: string;
  };
};

type LocalesType = {
  [key: string]: LocaleType;
};

export const locales: LocalesType = {
  'en-US': en,
  'es-ES': es,
};
