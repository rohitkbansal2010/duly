import { RefObject, useCallback, useEffect } from 'react';

type UseDetectTouchOutsideProps<T> = {
  ref: RefObject<T>,
  onTriggered: (e: TouchEvent) => void
}

type ContainsType = ((t: Node | null) => boolean);
type RefObjectCurrentType = { contains: ContainsType }

export const useDetectTouchOutside = <T extends RefObjectCurrentType>({
  ref,
  onTriggered,
}: UseDetectTouchOutsideProps<T>) => {
  const touchListener = useCallback(
    (e: TouchEvent) => {
      if (ref && ref.current) {
        if (!(ref.current.contains(e.target as Node))) {
          onTriggered?.(e);
        }
      }
    },
    [ onTriggered, ref ]
  );

  useEffect(() => {
    document.addEventListener('touchstart', touchListener);

    return () => {
      document.removeEventListener('touchstart', touchListener);
    };
  }, [ touchListener ]);

  return ref;
};
