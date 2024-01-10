import debounce from 'lodash/debounce';
import { useEffect } from 'react';
import { useDispatch } from 'react-redux';

import { setViewportInterface } from '@redux/actions';

export const useWindowSize = () => {
  const dispatch = useDispatch();
  const root = document.getElementById('root');

  useEffect(() => {
    const handleResize = debounce(() => {
      const { clientWidth: width, clientHeight: height } = root!;
      dispatch(setViewportInterface({ width, height }));
    }, 100);

    window.addEventListener('resize', handleResize);

    handleResize();

    return () =>
      window.removeEventListener('resize', handleResize);
  }, [ dispatch, root ]);
};
