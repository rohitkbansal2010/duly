import StartCheckoutDetailsBar from '@components/start-checkout/checkout-details-bar';
import { ProviderDetailsType } from '@types';

import styles from './start-checkout-navbar.scss';


type StartCheckoutNavbarProps = {
  providerDetails?: ProviderDetailsType;
  heading: string;
  resultScreen?: boolean;
  time?: string;
  actionButtonText?: string;
  skipButtonText?: string;
  IconImage: string;
  isDetails: boolean;
  detailsprofileImage?: string;
  detailsdoctorType?: string;
  detailsdoctorName?: string;
  detailsproviderName?: string;
  detailslocation?: string;
  detailspincode?: string;
  detailspage?: string;
  providerType?: string;
  changeProviderStyle?: string;
  referralIcon?: boolean; // TODO need to be removed after testing
  onAction?: () => void;
  onSkip?: () => void;
  handleDetailsProviderName?: () => void;
  selectedAppType?: number;
  setSelectedAppType?: React.Dispatch<React.SetStateAction<number>>;
  selectedMeetingType?: string;
  setSelectedMeetingType?: React.Dispatch<React.SetStateAction<string>>;
  detailsData?: {
    topLine: string,
    middleLine: string,
    bottomLine: string,
  }[];
  handleChangeLocation?: () => void;
  primaryButtonDisable?: boolean;
};

const StartCheckoutNavbar = ({
  detailsData,
  providerDetails,
  heading,
  resultScreen,
  time,
  actionButtonText,
  skipButtonText,
  IconImage,
  isDetails,
  detailsprofileImage,
  detailsdoctorType,
  detailslocation,
  detailspincode,
  detailspage,
  providerType,
  changeProviderStyle,
  referralIcon,
  onAction,
  onSkip,
  handleDetailsProviderName,
  selectedAppType,
  setSelectedAppType,
  selectedMeetingType,
  setSelectedMeetingType,
  handleChangeLocation,
  primaryButtonDisable,
}: StartCheckoutNavbarProps) => {
  const getAutomationId = (text: string | undefined):string =>{
    if(heading.includes('Follow')) return `checkout-navbar-${text?.toLowerCase()}-follow-up`;
    else if(heading === 'Labs') return `checkout-navbar-${text?.toLowerCase()}-labs`;
    else if(heading === 'Prescriptions') return `checkout-navbar-${text?.toLowerCase()}-prescriptions`;
    else if (heading.includes('Imaging')) return `checkout-navbar-${text?.toLowerCase()}-imaging`;
    else if (heading.includes('Referral')) return `checkout-navbar-${text?.toLowerCase()}-referral`;
    return 'checkout-navbar';
  };
  return (

    <>
      <div className={styles.mainDiv}>
        <div className={styles.childDiv}>
          <div className={styles.addChat}>
            {/* TODO need to be removed after testing */}
            {
            referralIcon ? (
              <img src={IconImage} alt="add chat" className={styles.referralIcon} />
            ) :
              <img src={IconImage} alt="add chat" />
          }
            <h3 className={styles.heading} data-automation={getAutomationId('heading')} >
              {heading}
            </h3>
          </div>
          <div className={styles.buttons} >
            {skipButtonText && (
              <button
                className={styles.buttonSecondary}
                data-automation={getAutomationId(skipButtonText)}
                onClick={onSkip}
              >
                {skipButtonText}
              </button>
            )}
            {actionButtonText &&
            ((time === '' || primaryButtonDisable) ? (
              <button
                disabled
                className={styles.buttonPrimary + ' ' + styles.disabledPrimaryButton}
                data-automation={getAutomationId(actionButtonText)}
              >
                {actionButtonText}
              </button>
            ) : (
              <button
                className={styles.buttonPrimary}
                onClick={onAction}
                data-automation={getAutomationId(actionButtonText)}
              >
                {actionButtonText}
              </button>
            ))}
          </div>
        </div>
        {isDetails &&
        detailsdoctorType &&
        detailspage?.includes('Follow-Up Appointments') && (
          <StartCheckoutDetailsBar
            providerDetails={providerDetails}
            profileImage={detailsprofileImage}
            resultScreen={resultScreen}
            doctorType={detailsdoctorType}
            page={detailspage}
            heading = {heading}            
          />
        )}
        {isDetails &&
        detailspage !== undefined &&
        !detailspage.includes('Follow-Up Appointments') && (
          <StartCheckoutDetailsBar
            providerDetails={providerDetails}
            resultScreen={resultScreen}
            profileImage={detailsprofileImage}
            doctorType={detailsdoctorType}
            location={detailslocation}
            pincode={detailspincode}
            page={detailspage}
            changeProviderStyle={changeProviderStyle}
            providerType={providerType}
            handleProvider={handleDetailsProviderName}
            selectedAppType={selectedAppType}
            setSelectedAppType={setSelectedAppType}
            selectedMeetingType={selectedMeetingType}
            setSelectedMeetingType={setSelectedMeetingType}
            heading = {heading}
            detailsData = {detailsData}
            handleChangeLocation = {handleChangeLocation}
          />
        )}
      </div>
    </>
  );};

export default StartCheckoutNavbar;
