import { noop } from 'lodash';

import { Avatar } from '@components/avatar';
import { CHECKOUT_DETAILS_BAR_PHOTO_WIDTH } from '@constants';
import { ProviderDetailsType } from '@types';
import { firstLetterCapital, formatAddressBottomLine, getPractitionersSuffix } from '@utils';

import styles from './checkout-details-bar.scss';
import { RightSideDetailsBar } from './right-side-details-bar';

type StartCheckoutDetailsBarProps = {
  providerDetails?: ProviderDetailsType;
  resultScreen?: boolean;
  profileImage?: string;
  doctorType?: string;
  doctorName?: string;
  providerName?: string;
  location?: string;
  pincode?: string;
  page: string;
  providerType?: string;
  changeProviderStyle?: string;
  selectedAppType?: number;
  selectedMeetingType?: string;
  heading?:string;
  setSelectedAppType?: React.Dispatch<React.SetStateAction<number>>;
  handleProvider?: () => void;
  setSelectedMeetingType?: React.Dispatch<React.SetStateAction<string>>;
  detailsData?: {
    topLine: string,
    middleLine: string,
    bottomLine: string,
  }[];
  handleChangeLocation?: () => void;
};

export const appointmentType = [
  {
    name: '2-Week Exam',
    val: 0.5,
  },
  {
    name: '3-Month Exam',
    val: 3,
  },
  {
    name: '6-Month Exam',
    val: 6,
  },
  {
    name: '1-Year Exam',
    val: 12,
  },
];

const StartCheckoutDetailsBar = (props: StartCheckoutDetailsBarProps) => {
  const ChangeProvider = () => {
    if(props.handleProvider !== undefined){
      return 'Change Provider';
    }
    else if(props.providerDetails?.humanName.suffixes)
      return getPractitionersSuffix(props.providerDetails 
        && props.providerDetails.humanName?.suffixes);
  };
  
  const ProviderSuffix = () => {
    if(props.providerDetails){
      return (props.providerDetails.humanName?.suffixes 
        && getPractitionersSuffix(props.providerDetails.humanName?.suffixes));
    }
  };

  const checkProviderStyle = () => {
    if(props.changeProviderStyle) {
      return (
        <div
          className={styles.chooseProviderLink}
          onKeyDown={noop}
          role="button"
          tabIndex={-1}
          onClick={props.handleProvider}
        >
          {ChangeProvider()}
        </div>
      );
    } else {
      return (
        <div
          className={styles.data}
          onKeyDown={noop}
          tabIndex={-1}
          role="button"
          onClick={props.handleProvider}
        >
          {ProviderSuffix()}
        </div>
      );
    }
  };
  if(!props.heading?.includes('Imaging')){
    return props.providerDetails ? (
      <div className={styles.mainDiv}>
        <div className={styles.doctorDetails}>
          <div className={styles.doctorInfo}>
            <Avatar 
              src={props.providerDetails.photo.url} 
              alt={props.providerDetails.humanName.familyName}
              width={CHECKOUT_DETAILS_BAR_PHOTO_WIDTH}
            />
            <div>
              <div className={styles.doctorStream}>{props.providerDetails.specialty}</div>
              <div className={styles.doctorName}>
                Dr. {props.providerDetails.humanName.familyName}
              </div>
              {checkProviderStyle()}
            </div>
          </div>
          {props.providerDetails.location && !props.resultScreen && (
          <div className={styles.location}>
            <div className={styles.locationTitle}>LOCATION </div>
            <div className={styles.address}>
              {firstLetterCapital(props.providerDetails.location.address.addressLine)}
            </div>
            <div className={styles.pincode}>
              {firstLetterCapital(formatAddressBottomLine(
                {
                  city: props.providerDetails.location.address.city,
                  postalCode: props.providerDetails.location.address.zipCode,
                  state: props.providerDetails.location.address.state,
                }
              ))}
            </div>
          </div>
        )}
          <RightSideDetailsBar 
            page={props.page}
            setSelectedAppType={props.setSelectedAppType}
            setSelectedMeetingType={props.setSelectedMeetingType}
            providerType={props.providerType}
          />
        </div>
      </div>
    ) : <></>;
  }else{
    return (
      <div className={styles.mainDiv}>
        <div className={styles.doctorDetailsImaging} >
          {props.detailsData && props.detailsData.map((data, index) => 
            (
              <div className={styles.doctorInfo} key={index}>
                <div className={styles.location} key={index}>
                  <div className={styles.locationTitle}>{data.topLine}</div>  
                  <div className={styles.address}>
                    {data.middleLine}
                  </div>
                  {data.bottomLine === 'Change Location' ? (
                    <div 
                      className={styles.changeLocation}
                      onClick={() => 
                        props.handleChangeLocation && props.handleChangeLocation()}
                      role="button"
                      tabIndex={-1}
                      onKeyDown={noop}
                      data-automation="change-location-button"
                    >
                      {data.bottomLine}
                    </div>
                  ) : 
                  (
                    <div className={styles.pincode}>
                      {data.bottomLine}
                    </div>
                  )}
                </div>
              </div>
            ))}
        </div>
      </div>
    );
  }
};

export default StartCheckoutDetailsBar;
