import { AllergiePlateSkeleton } from '@ui-kit/allergie-plate';

import styles from './allergies.module.scss';

const SKELETON_COUNT = 6;

export const AllergiesWidgetSkeleton = () =>
  (
    <div className={styles.allergiesWidgetSkeleton}>
      {[ ...new Array(SKELETON_COUNT) ].map((_, index) =>
        <AllergiePlateSkeleton key={index} />)}
    </div>
  ); 
