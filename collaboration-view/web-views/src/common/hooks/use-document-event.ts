import { useEffect } from 'react';

export const useDocumentEvent = <K extends keyof WindowEventMap>(
  callback: (event: WindowEventMap[K]) => void,
  eventType: null | K
): void => {
  useEffect(() => {
    if (eventType) {
      window.addEventListener(eventType, callback);

      return () => {
        window.removeEventListener(eventType, callback);
      };
    }
  }, [ callback, eventType ]);
};
