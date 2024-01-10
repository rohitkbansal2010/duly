import noop from 'lodash/noop';
import {
  MouseEventHandler,
  useCallback,
  useMemo,
  useRef,
  useState,
  TouchEvent,
  useEffect
} from 'react';
import { useSelector } from 'react-redux';

import { RootState } from '@redux/reducers';
import { WidgetDataType } from '@types';

import styles from './nav-secondary.module.scss';

export type NavSecondaryProps = {
  widgets: WidgetDataType[];
  activeWidget?: WidgetDataType;
  onWidgetSelected: (w: WidgetDataType) => void;
};

type SliderPositionsItem = {
  left: number;
  width: number;
};

const defaultTouchCoords = { clientX: 0, clientY: 0 };
const defaultSliderPosition = { left: 0, width: 0 };

const isIntersected = (x: number, y: number, rect: DOMRect) =>
  x >= rect.x && x <= rect.x + rect.width && y >= rect.y && y <= rect.y + rect.height;

export const NavSecondary = ({
  widgets = [],
  activeWidget,
  onWidgetSelected = noop,
}: NavSecondaryProps) => {
  const [ sliderPositions, setSliderPositions ] = useState<Record<string, SliderPositionsItem>>({});

  const remCoefficient = useSelector(({ UI }: RootState) =>
    UI.remCoefficient);

  const fontSize = useSelector(({ APPSTATE }: RootState) =>
    APPSTATE?.fontSize);

  const height = useSelector(({ UI }: RootState) =>
    UI.height);

  const containerClassname = []
    .concat(styles.navSecondaryContainer)
    .join(' ');

  const widgetsRefs = useRef<HTMLDivElement[]>([]);

  const onTouchAction = useCallback(
    (e: TouchEvent) => {
      const { clientX: x, clientY: y } = e?.touches?.[0] || defaultTouchCoords;

      widgetsRefs.current?.map((node) => {
        if (!node) return;

        const rect = node.getBoundingClientRect();
        const route = node.dataset.key!;

        if (isIntersected(x, y, rect)) {
          const foundWidget = widgets.find(widget =>
            widget.route === route);

          if (foundWidget && activeWidget?.route !== foundWidget.route) {
            onWidgetSelected(foundWidget);
          }
        }
      });
    },
    [ activeWidget?.route, onWidgetSelected, widgets ]
  );

  const calcSliderPositions = () => {
    const positions = widgets.reduce((acc, widget) => {
      const node = widgetsRefs.current.find(node =>
        node.dataset.key === widget.route);

      const { width } = node?.getBoundingClientRect() || { width: 0 };
      const left = Object.values(acc).reduce((sum, accItem) =>
        sum + accItem.width, 0);
      return {
        ...acc,
        [widget.route]: { width, left },
      };
    }, {} as Record<string, SliderPositionsItem>);

    setSliderPositions(positions);
  };

  const activeSiderPosition = useMemo(
    () =>
      (activeWidget ? sliderPositions[activeWidget.route] : defaultSliderPosition),
    [ sliderPositions, activeWidget ]
  );

  const addItemRef = (node: HTMLDivElement) => {
    if (!node) return;

    widgetsRefs.current.push(node);

    // When all refs are added
    if (widgetsRefs.current.length == widgets.length && widgets.length > 0) {
      calcSliderPositions();
    }
  };

  const handleItemClick: MouseEventHandler = (e) => {
    const { target } = e;

    const clickedWidget = widgets.find(
      widget =>
        widget.route === (target as HTMLDivElement).dataset.key!
    );

    if (target && clickedWidget) {
      onWidgetSelected(clickedWidget);
    }
  };

  const itemRenderer = (widget: WidgetDataType, idx: number) => {
    const className = `${styles.navSecondaryItem} ${widget.route === activeWidget?.route ? styles.navSecondaryItemActive : ''
    }`;

    return (
      <div
        role="button"
        tabIndex={idx + 1}
        onKeyDown={noop}
        key={widget.route}
        data-key={widget.route}
        ref={addItemRef}
        className={className}
        onClick={handleItemClick}
      >
        {widget.navTitle}
      </div>
    );
  };

  useEffect(() => {
    calcSliderPositions();
  }, [ remCoefficient, height, fontSize ]);

  useEffect(() => {
    setTimeout(() => {
      calcSliderPositions();
    }, 1000);
  }, []);

  return (
    <div
      onTouchMove={onTouchAction}
      onTouchStart={onTouchAction}
      onTouchEnd={onTouchAction}
      className={containerClassname}
    >
      <div
        className={styles.navSecondarySlider}
        style={{
          left: activeSiderPosition.left,
          width: activeSiderPosition.width + 'px',
        }}
      />
      {widgets.map(itemRenderer)}
    </div>
  );
};
