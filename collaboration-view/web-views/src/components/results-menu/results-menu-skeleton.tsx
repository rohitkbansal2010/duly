import { ResultsMenuItemSkeleton } from './results-menu-item';
import styles from './results-menu.scss';

export const ResultsMenuSkeleton = () =>
  (
    <div className={styles.resultsMenuSkeleton}>
      {[ ...new Array(3) ].map((_, index) =>
        (
          <ResultsMenuItemSkeleton key={index} />
        ))}
    </div>
  );
