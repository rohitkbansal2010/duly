import { FadedScroll } from '@components/faded-scroll-2';

import styles from './tests-list.scss';

type TestsListProps = {
  title:JSX.Element;
  tests: string[];
  step?: number;
};

export const TestsList = ({ title, tests, step }: TestsListProps) => 
{
  const TestData = () => 
    (
      <div className={styles.TestsListData}>
        {
        step === 0 || step ?
          (
            <div className={styles.TestsListDataItem}>
              {tests[step]}
            </div>
          ) :
        (tests?.map((test:string, index: number) => 
        (
          
          <div className={styles.TestsListDataItem} key={index}>
            {test}
          </div>
        )))}
        
      </div>
    );

  return (
    <div className={styles.TestsList}>     
      {title}
      {!(tests?.length > 2 && step !== undefined && step >= 0) ? 
      (
        <FadedScroll height="12rem" className={styles.TestsListScroll} >
          <TestData />
        </FadedScroll>
      ) :
      (
        <TestData />
      )
    }
    </div>
  );
};
