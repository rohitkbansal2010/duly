import throttle from 'lodash/throttle';
import {
  useState, useMemo, useEffect, RefObject
} from 'react';

const THROTTLE_SCROLL_DELAY = 0;
const FADE_THRESHOLD = 10;
const FADE_THRESHOLD_QUARTER_STEP = 50;

type TopFadeClassname = 'top0' | 'top25' | 'top50' | 'top75' | 'top100';
type BottomFadeClassname = 'bottom0' | 'bottom25' | 'bottom50' | 'bottom75' | 'bottom100';

const getTopFadeClassname = (scrollTop: number) => {
  const pureScrollTop = scrollTop - FADE_THRESHOLD;

  switch(true) {
    case pureScrollTop <= 0: return 'top0';
    case pureScrollTop <= FADE_THRESHOLD_QUARTER_STEP: return 'top25';
    case pureScrollTop <= 2 * FADE_THRESHOLD_QUARTER_STEP: return 'top50';
    case pureScrollTop <= 3 * FADE_THRESHOLD_QUARTER_STEP: return 'top75';
    default: return 'top100';
  }
};

const getBottomFadeClassname = (scrollTop: number, offsetHeight: number, scrollHeight: number) => {
  const pureScrollHeight = scrollHeight - FADE_THRESHOLD;
  const contentOffset = offsetHeight + scrollTop;

  const treshold = pureScrollHeight - contentOffset;

  switch(true) {
    case treshold <= 0: return 'bottom0';
    case treshold <= FADE_THRESHOLD_QUARTER_STEP: return 'bottom25';
    case treshold <= 2 * FADE_THRESHOLD_QUARTER_STEP: return 'bottom50';
    case treshold <= 3 * FADE_THRESHOLD_QUARTER_STEP: return 'bottom75';
    default: return 'bottom100';
  }
};

export const useScrollFades = <T extends HTMLElement>(ref: RefObject<T>) => {
  const [ topFadeClassname, setTopFadeClassname ] = useState<TopFadeClassname>('top0');
  const [ bottomFadeClassname, setBottomFadeClassname ] = useState<BottomFadeClassname>('bottom0');

  const throttledScrollListener = useMemo(
    () =>
      throttle(() => {
        if (ref.current) {
          const { scrollTop, offsetHeight, scrollHeight } = ref.current;

          setTopFadeClassname(getTopFadeClassname(scrollTop));
          setBottomFadeClassname(getBottomFadeClassname(scrollTop, offsetHeight, scrollHeight));
        }
      }, THROTTLE_SCROLL_DELAY),
    [ ref ]
  );

  useEffect(() => {
    if (ref.current) {
      const { scrollTop, offsetHeight, scrollHeight } = ref.current;
      setTopFadeClassname(getTopFadeClassname(scrollTop));
      setBottomFadeClassname(getBottomFadeClassname(scrollTop, offsetHeight, scrollHeight));
    }
  }, [ ref ]);

  useEffect(() => {
    const container = ref.current;

    container?.addEventListener('scroll', throttledScrollListener);

    return () =>
      container?.removeEventListener('scroll', throttledScrollListener);
  }, [ ref, throttledScrollListener ]);

  useEffect(() => {
    if (ref.current) {
      const observer = new MutationObserver(throttledScrollListener);
      observer.observe(ref.current, { attributes: true, childList: true, subtree: true });

      return () =>
        observer.disconnect();
    }
  }, [ ref, throttledScrollListener ]);

  return { topFadeClassname, bottomFadeClassname };
};
