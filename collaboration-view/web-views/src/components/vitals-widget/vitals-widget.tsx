import { useState } from 'react';

import { VitalsCards } from '@components/vitals-cards';
import { VitalsChart } from '@components/vitals-chart';

import styles from './vitals-widget.scss';


export const VitalsWidget = () =>{
  const [ toggle, setToggle ] = useState(sessionStorage.getItem('metric') && sessionStorage.getItem('metric') === 'on' ? 'on' : 'off');
  return (
    <div className={styles.vitalsWidgetContainer}>
      <VitalsCards toggle={toggle} />
      <VitalsChart toggle={toggle} setToggle={setToggle}/>
    </div>
  );};
