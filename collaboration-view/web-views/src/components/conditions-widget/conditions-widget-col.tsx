import { FadedScroll } from '@components/faded-scroll-2';
import { ConditionPlate } from '@ui-kit/condition-plate';

import styles from './conditions-widget.module.scss';
import { NoConditionsWidget } from './no-conditions';

export type ConditionType = {
  title: string;
  date?: string;
};

type ConditionsWidgetColProps = {
  isCurrent: boolean;
  title: string;
  conditions: ConditionType[];
};

export const ConditionsWidgetCol = ({
  isCurrent,
  title,
  conditions = [],
}: ConditionsWidgetColProps) =>
  (
    <div className={styles.conditionsWidgetCol}>
      <div>
        <h2 className={styles[isCurrent ? 'conditionsWidgetColCurrentTitle' : 'conditionsWidgetColTitle']}>
          {title}
        </h2>
      </div>
      <div className={styles.conditionsWidgetColContent}>
        {conditions.length
        ? (

          <FadedScroll height="100%">
            <div className={styles.conditionsWidgetColData}>
              {conditions.map(({ date, title: plateTitle }) =>
                  (
                    <ConditionPlate
                      isCurrent={isCurrent}
                      key={plateTitle}
                      title={plateTitle}
                      date={date}
                    />
                  ))}
            </div>
          </FadedScroll>
        )
        : <NoConditionsWidget isCurrent={isCurrent} />
      }
      </div>
    </div>
  );


