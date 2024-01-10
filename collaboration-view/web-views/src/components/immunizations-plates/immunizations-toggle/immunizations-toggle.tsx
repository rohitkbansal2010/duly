import noop from 'lodash/noop';
import { PropsWithChildren } from 'react';

import { CustomAccordionToggle } from '@components/custom-accordion-toggle';
import { ImmunizationsToggles } from '@enums';
import { expandIcon, minusIcon, plusIcon } from '@icons';

import styles from './immunizations-toggle.scss';

type ImmunizationsToggleProps = PropsWithChildren<{
  eventKey: string;
  type?: ImmunizationsToggles;
  callback?: (isCurrentEventKey: boolean) => void;
}>

export const ImmunizationsToggle =
  ({
    eventKey,
    type = ImmunizationsToggles.GROUP,
    callback = noop,
    children,
  }: ImmunizationsToggleProps) => {
    const getToggleImgClasses = (isCurrentEventKey: boolean): string =>
      [ styles.toggleButtonImage ]
        .concat(isCurrentEventKey ? styles.collapsedToggle : styles.expandedToggle)
        .join(' ');

    return (
      <CustomAccordionToggle
        eventKey={eventKey}
        callback={callback}
      >
        {(decoratedOnClick, isCurrentEventKey) =>
          (
            <button
              type="button"
              className={styles.toggleButton}
              onClick={decoratedOnClick}
            >
              {children}
              {type === ImmunizationsToggles.GROUP
                ? (
                  <img
                    src={isCurrentEventKey ? minusIcon : plusIcon}
                    className={styles.toggleButtonImage}
                    alt="expand collapse button"
                  />
                )
                : (
                  <img
                    src={expandIcon}
                    alt="expand collapse button"
                    className={getToggleImgClasses(isCurrentEventKey)}
                  />
                )}
            </button>
          )}
      </CustomAccordionToggle>
    );
  };
