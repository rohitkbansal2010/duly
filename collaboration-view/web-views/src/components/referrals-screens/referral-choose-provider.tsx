import { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';

import { ChooseProvider } from '@components/choose-provider-modal/ChooseProvider';
import { ChooseProviderData } from '@mock-data';
import { RootState } from '@redux/reducers';

import GroupIcon from '../../common/icons/mock/Group-icon.svg';

import styles from './ReferralsInsider.scss';


type ReferralsInsiderProps = {
  setReferralKey: React.Dispatch<React.SetStateAction<string>>;
  setProvider: React.Dispatch<any>;
  providerType: string;
  setLocation: React.Dispatch<React.SetStateAction<string>>;
  setPincode: React.Dispatch<React.SetStateAction<string>>;
  location: string;
  pincode: string;
  referralVisit: string;
};


export const Referrals_Insider = (props: ReferralsInsiderProps) => {
  const [ isOpen, setIsOpen ] = useState(false);
  const [ providerDetails, setProviderDetails ] = useState<any>();
  const { errorTitle } = useSelector(({ APPSTATE }: RootState) =>
    APPSTATE.ScheduleAppointmentErrorData);


  const toggleModal = () => {
    setIsOpen(!isOpen);
  };
  useEffect(()=>{
    if(errorTitle === 'Bad Network!' && isOpen){
      setIsOpen(false);
    }
  }, [ errorTitle ]);

  useEffect(() => {
    props.setProvider(providerDetails);
  }, [ providerDetails ]);

  useEffect(() => {
    props.setLocation(props.location);
    props.setPincode(props.pincode);
  }, [ props.location, props.pincode ]);


  return (
    <>
      <div className={styles.mainCompoBox}>
        <ChooseProvider
          title="Choose Provider"
          titleImage={GroupIcon}
          ChooseProviderData={ChooseProviderData}
          isOpen={isOpen}
          toggleModal={toggleModal}
          setReferralKey={props.setReferralKey}
          setProviderDetails={setProviderDetails}
          setLocation={props.setLocation}
          setPincode={props.setPincode}
          providerType={props.providerType}
          step="Referral"
        />
        <div className={styles.headerTextDiv}>
          <p className={styles.MainText}>{props.providerType}</p>
        </div>

        <div className={styles.mainParaDiv}>
          <p className={styles.mainParaText}>
            {' '}
            A Provider was not suggested by your doctor. <br /> Let’s look at some providers who
            have availability!{' '}
          </p>
        </div>

        <button
          className={styles.MainButton}
          onClick={() => {
            props.setLocation('');
            props.setPincode('');
            toggleModal();
          }}
        >
          <p className={styles.buttonText}>Choose Provider</p>
        </button>
      </div>
    </>
  );
};
